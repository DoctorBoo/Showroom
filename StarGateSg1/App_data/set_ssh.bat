echo "HTTP Error 503. The service is unavailable"? You can't use OWIN with a start option URL and also have a URL reservation, 
echo it's one or the other. Best option would probably be to pass in the endpoint from a config.
echo netsh http add urlacl url=https://+:44322/ user=iedereen


netsh http add sslcert ipport=0.0.0.0:44322 certhash=dc947515ee7ac43a01d136b818f8335f8624f027  appid={214124cd-d05b-4309-9af9-9caa44b2b74a}