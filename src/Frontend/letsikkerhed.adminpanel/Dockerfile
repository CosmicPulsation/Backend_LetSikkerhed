FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
# Prefer reproducible installs; fall back if lockfile is out of sync.
RUN npm ci || npm install

COPY . .

# Bind to all interfaces so the host can reach the dev server.
ENV HOST=0.0.0.0
ENV PORT=3000
# Disable auto-opening a browser inside the container (prevents Windows path spawn errors).
ENV BROWSER=none

EXPOSE 3000

CMD ["npm", "start"]
