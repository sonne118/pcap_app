@echo off
SERVER_CN=localhost
set OPENSSL_CONF=c:\OpenSSL-Win64\bin\openssl.cfg

openssl genpkey -algorithm RSA -out ca.key -pkeyopt rsa_keygen_bits:4096
openssl req -x509 -new -nodes -key ca.key -sha256 -days 365 -out ca.crt -subj "/C=US/ST=State/L=City/O=Organization/OU=OrgUnit/CN=example.com"

openssl genpkey -algorithm RSA -out server.key -pkeyopt rsa_keygen_bits:4096
openssl req -new -key server.key -out server.csr -subj "/C=US/ST=State/L=City/O=Organization/OU=OrgUnit/CN=server.example.com"

openssl x509 -req -in server.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out server.crt -days 365 -sha256

openssl genpkey -algorithm RSA -out client.key -pkeyopt rsa_keygen_bits:4096

openssl req -new -key client.key -out client.csr -subj "/C=US/ST=State/L=City/O=Organization/OU=OrgUnit/CN=client.example.com"

openssl x509 -req -in client.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out client.crt -days 365 -sha256

openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt

openssl pkcs12 -export -out client.pfx -inkey client.key -in client.crt