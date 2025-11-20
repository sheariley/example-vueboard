# MailDev Local SMTP Server

This folder contains configuration to run MailDev for local email development.

## Usage

1. Make sure Docker is installed and running.
2. In this folder, run:
   
   ```pwsh
   docker compose up -d
   ```

3. Access the MailDev web UI at [http://localhost:1080](http://localhost:1080).
4. Configure your app to use SMTP server at `localhost:1025`.

## Stopping MailDev

```pwsh
docker compose down
```

---
MailDev docs: https://github.com/maildev/maildev
