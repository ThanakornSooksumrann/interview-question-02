# Stage 1: Build
FROM node:20-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm ci --legacy-peer-deps

COPY . .
RUN npm run build -- --configuration development

# Stage 2: Serve static files
FROM node:20-alpine AS runner

RUN npm install -g serve

COPY --from=builder /app/dist/example-webapp/browser ./dist

EXPOSE 4000

CMD ["serve", "-s", "dist", "-l", "4000"]
