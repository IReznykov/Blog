#!/bin/bash

# Використання
# ./assume-role.sh --profile 123456789012_AdministratorAccess --role TargetRoleName [-duration 2]
# або
# ./assume-role.sh --profile iamuser_name --account 123456789012 --role TargetRoleName [-duration 2]

set -euo pipefail

# Перевірка залежностей
command -v aws >/dev/null 2>&1 || { echo "❌ aws CLI не знайдено"; exit 1; }
command -v jq  >/dev/null 2>&1 || { echo "❌ jq не знайдено"; exit 1; }

usage() {
    echo "Використання:"
    echo "  $0 --role <role-name> [--profile <profile=default>] [--account <account-id>] [--duration <hours=1>]"
    exit 1
}

# Значення по замовчуванню
PROFILE="default"
ACCOUNT_ID=""
ASSUME_ROLE_NAME=""
DURATION=1

# Аналізуємо аргументи - генеруємо помилки
# якщо знайшли непередбачуваний аргумент
while [[ $# -gt 0 ]]; do
    key="$1"
    case $key in
        --profile)
            PROFILE="$2"
            shift 2 ;;
        --account)
            ACCOUNT_ID="$2"
            shift 2 ;;
        --role)
            ASSUME_ROLE_NAME="$2"
            shift 2 ;;
        --duration)
            DURATION="$2"
            shift 2 ;;
        *)
            echo "❌ Невідомий параметр: $1"
            usage ;;
    esac
done

# Профіль може мати ім'я по замовчуванню default
if [[ -z "${PROFILE:-}" ]]; then
    PROFILE="default"
fi
# Перевіряємо порожнє значення для тривалості повноважень ролі
if [[ -z "${DURATION:-}" ]]; then
    DURATION=1
fi
# Ім'я ролі обов'язкове
[[ -z "${ASSUME_ROLE_NAME:-}" ]] && usage
# Якщо ідентифікатор акаунту не надано окремим параметром, намагаємось
# його відокремити з імені профілю (припускаємо формат accountId_)
if [[ -z "${ACCOUNT_ID:-}" ]]; then
    if [[ "$PROFILE" =~ ^([0-9]{12})_ ]]; then
        ACCOUNT_ID="${BASH_REMATCH[1]}"
    fi
    if [[ ! "$ACCOUNT_ID" =~ ^[0-9]{12}$ ]]; then
        echo "❌ Неможливо визначити Account ID із імені профіля '$PROFILE'"
        echo "   Очікуваний формат: 123456789012_name"
        exit 1
    fi
fi

ROLE_ARN="arn:aws:iam::$ACCOUNT_ID:role/$ASSUME_ROLE_NAME"
DURATION_SECONDS=$((DURATION * 3600))

# Приймаємо роль, помилки виводимо користувачу
TMP_ERR=$(mktemp)
CREDENTIALS=$(aws sts assume-role \
    --profile "$PROFILE" \
    --role-arn "$ROLE_ARN" \
    --role-session-name "AssumeRoleSession" \
    --duration-seconds "$DURATION_SECONDS" \
    --output json 2>"$TMP_ERR" || true)

if [[ -z "$CREDENTIALS" ]]; then
    echo "❌ Помилка при прийнятті ролі $ASSUME_ROLE_NAME в акаунті $ACCOUNT_ID"
    cat "$TMP_ERR"   # показати помилку
    rm -f "$TMP_ERR" # видалити файл
    exit 1
fi

AWS_ACCESS_KEY_ID=$(echo "$CREDENTIALS" | jq -r '.Credentials.AccessKeyId')
AWS_SECRET_ACCESS_KEY=$(echo "$CREDENTIALS" | jq -r '.Credentials.SecretAccessKey')
AWS_SESSION_TOKEN=$(echo "$CREDENTIALS" | jq -r '.Credentials.SessionToken')
EXPIRATION=$(echo "$CREDENTIALS" | jq -r '.Credentials.Expiration')

cat <<EOF

aws_access_key_id=$AWS_ACCESS_KEY_ID
aws_secret_access_key=$AWS_SECRET_ACCESS_KEY
aws_session_token=$AWS_SESSION_TOKEN

# Закінчується в: $EXPIRATION
EOF
