# Stage 1: Build the application
FROM node:20 AS builder

WORKDIR /app

# Install system dependencies
RUN apt-get update && \
   apt-get install -y python3 make g++ && \
   npm install -g sass

# Copy package files
COPY frontend/client-app/package*.json ./

# Install project dependencies
RUN npm install

# Copy source
COPY frontend/client-app/ .

# Build
RUN npm run build

# Stage 2: Nginx for serving built files (Production)
FROM nginx:alpine

# Install bash for advanced scripting or debugging (optional)
RUN apk --no-cache add bash

# Copy built assets from builder stage
COPY --from=builder /app/dist /usr/share/nginx/html

# Copy nginx configuration
COPY frontend/client-app/nginx.conf /etc/nginx/conf.d/default.conf

# Copy SSL certificates into the container
# COPY frontend/client-app/certs/ /etc/nginx/ssl/

# Expose port
EXPOSE 80 443

# Start nginx
CMD ["nginx", "-g", "daemon off;"]