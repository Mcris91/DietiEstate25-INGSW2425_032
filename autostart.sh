#!/bin/bash

# Definizione colori per l'output
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo "Verifica installazione Docker in corso..."

# Controlla se il comando 'docker' esiste
if command -v docker &> /dev/null; then
    echo -e "${GREEN}Docker trovato! Avvio dei container...${NC}"

    sudo docker compose up -d --build

    if [ $? -eq 0 ]; then
        echo -e "${GREEN}Fatto! I container sono in esecuzione.${NC}"
        echo -e "${GREEN}Il client e' raggiungibile all'indirizzo http://localhost:8080 (se la porta di default non e' stata modificata)."
    else
        echo -e "${RED}Si è verificato un errore durante l'avvio di Docker Compose.${NC}"
    fi
else
    echo -e "${RED}ERRORE: Docker non sembra essere installato su questo sistema.${NC}"
    echo "Per favore installa Docker Desktop o Docker Engine prima di continuare."
    exit 1
fi
