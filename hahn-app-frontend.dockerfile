FROM node:18-alpine AS build

WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY HanhDeliveryFront/package*.json ./

# Install dependencies
RUN npm install

# Copy the entire project
COPY HanhDeliveryFront/ .

# Build the Angular app for production
RUN npm run build

# Use NGINX as the production server
FROM nginx:alpine

# Copy the built app from the 'build' stage to the NGINX HTML directory
COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start NGINX server
CMD ["nginx", "-g", "daemon off;"]