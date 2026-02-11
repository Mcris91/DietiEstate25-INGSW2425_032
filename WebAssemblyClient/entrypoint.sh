#!/bin/sh

if [ ! -z "$API_BASE_URL" ]; then
  echo "Setting API_BASE_URL: $API_BASE_URL"
  echo "{ \"ApiBaseUrl\": \"$API_BASE_URL\" }" > /usr/share/nginx/html/appsettings.json
fi

# Avvia Nginx
exec nginx -g "daemon off;"