# Design Spec: E-commerce Frontend for IMDB Microservices

**Date:** 2026-05-12
**Status:** Approved
**Topic:** Simple React Frontend with Admin Panel and Storefront

## 1. Overview
A modern, responsive web application serving as the frontend for the existing IMDB Microservices ecosystem. It provides two main experiences: a customer-facing storefront and an administrative management panel.

## 2. Technical Stack
- **Framework:** React (TypeScript) via Vite
- **Styling:** Tailwind CSS (Vanilla CSS for custom components)
- **State Management:**
  - **Global/Server State:** TanStack Query (React Query)
  - **Local State (Cart, Auth):** Zustand
- **Networking:** Axios with interceptors for JWT injection
- **Routing:** React Router DOM

## 3. Architecture (Modular Monolith)
The app is organized by features to maintain clarity:
- `api/`: Centralized Axios instance targeting the YARP Gateway (`localhost:8000`).
- `features/auth/`: Login/Logout logic and JWT handling.
- `features/catalog/`: Product listing and category filtering.
- `features/admin/`: CRUD operations for products and order overview.
- `features/orders/`: Cart management, checkout flow, and payment simulation.

## 4. Key Features & Flows

### 4.1. Storefront (Public/Customer)
- **Product Catalog:** Fetches products from `catalog-service`.
- **Shopping Cart:** Persistent local cart using Zustand.
- **Checkout:** authenticated flow to create orders in `orders-service`.
- **Payment Simulation:** A view to "process" payments by updating order status.

### 4.2. Admin Panel (Protected)
- **Dashboard:** Overview of system status.
- **Product Management:** Full CRUD (Create, Read, Update, Delete) via `catalog-service`.
- **Order Monitoring:** View all orders placed in the system.

### 4.3. Authentication
- Login via `user-service/login`.
- JWT token stored in `localStorage`.
- Automatic redirection to `/login` for protected routes.

## 5. Data Flow & Integration
- **Gateway First:** All requests go through the YARP Gateway.
- **Optimistic Updates:** Admin panel uses React Query's optimistic updates or automatic invalidation for a snappy feel.
- **Error Handling:** Centralized error toasts for API failures (401 Unauthorized, 400 Bad Request).

## 6. Development & Deployment
- **Environment:** Optimized for WSL2 (0.0.0.0 binding for dev server).
- **Commands:** `npm run dev` for development, `npm run build` for production.

---
**Self-Review:**
- No "TBD" or "TODO" items.
- Architecture matches the microservices structure.
- Scope covers all user requirements (Shop, Admin, Orders, Payments).
- No ambiguity in routing or state management.
