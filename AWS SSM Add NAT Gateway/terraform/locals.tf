############################
# Locals
############################

locals {
  tags = {
    "ManagedBy"   = "Terraform",
    "Project"     = "Blog"
    "Environment" = var.environment
  }

  vpc_cidr = "172.18.0.0/18"

  subnet = {
    newbits        = 4
    public_offset  = 0
    private_offset = 10
  }

  azs = slice(data.aws_availability_zones.available.names, 0, 2)

  # NAT Gateways are NOT created in dev
  create_nat = var.environment != "dev"
  
  user_data = <<EOF
#!/bin/bash
set -eux

### 1. Packages
dnf install -y nginx python3 python3-pip
pip3 install flask requests gunicorn

### 2. App directory
mkdir -p /opt/status
cd /opt/status

### 3. Flask app
cat > app.py <<'PY'
from flask import Flask
import subprocess
import requests
import datetime

app = Flask(__name__)

def internet_ok():
    try:
        subprocess.check_output(
            ["curl", "-s", "--max-time", "2", "https://1.1.1.1"]
        )
        return True
    except:
        return False

def instance_id():
    token = requests.put(
        "http://169.254.169.254/latest/api/token",
        headers={"X-aws-ec2-metadata-token-ttl-seconds": "21600"}
    ).text

    return requests.get(
        "http://169.254.169.254/latest/meta-data/instance-id",
        headers={"X-aws-ec2-metadata-token": token}
    ).text

@app.route("/")
def index():
    iid = instance_id()[:8]
    ok = internet_ok()
    status = "ONLINE" if ok else "OFFLINE"
    color = "green" if ok else "red"

    return f"""
    <html>
    <body>
        <h1>Hello</h1>
        <p>I'm instance {iid}</p>
        <p>Internet status:
           <span style="color:{color}">{status}</span></p>
        <p>Checked at: {datetime.datetime.now()}</p>
    </body>
    </html>
    """

@app.route("/health")
def health():
    return "OK"
PY

### 4. systemd service
cat > /etc/systemd/system/status.service <<'SERVICE'
[Unit]
Description=EC2 Internet Status App
After=network.target

[Service]
User=nginx
WorkingDirectory=/opt/status
ExecStart=/usr/local/bin/gunicorn -b 127.0.0.1:5000 app:app
Restart=always

[Install]
WantedBy=multi-user.target
SERVICE

### 5. nginx reverse proxy
cat > /etc/nginx/conf.d/status.conf <<'NGINX'
server {
    listen 80;

    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_set_header Host $host;
    }
}
NGINX

### 6. Enable services
systemctl daemon-reload
systemctl enable nginx status
systemctl start nginx status
EOF
}