# Post Xunit and dynamic
There is a code for the post https://ireznykov.com/2016/09/26/xunit-and-dynamic/

#Abstract
In the post the object with several public properties taken from complex "parent" object is created; and its class is constructed on the fly via reflection. This code should be covered by unit tests. The main issue that the type of constructed object is not defined during compile time. Xunit library is used, and two different approaches is shown: the using dynamic type and TypeDescriptor class. 

#Disclaimer
1. All used IP-addresses, names of servers, workstations, domains, are fictional and are used exclusively as a demonstration only.
2. Information is provided «AS IS».