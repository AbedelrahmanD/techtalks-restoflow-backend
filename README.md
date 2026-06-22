# RestoFlow — Smart Dining & Kitchen Management System

RestoFlow is a comprehensive, full-cycle restaurant management application designed to digitize the entire dining experience, bridging the gap between customers, kitchen staff, and administrators through a synchronized real-time platform.

---

## 🚀 System Architecture & Tech Stack

* **Backend:** .NET
* **Database:** Microsoft SQL Server
* **Frontend:** Angular
* **Localization:** Out-of-the-box **LTR & RTL** compatibility supporting both **English and Arabic** languages.

---

## ✨ Core Features

### 1. Admin Dashboard
A comprehensive administrative panel allowing owners and managers to fully configure the operational flow of their restaurant:
* **Menu & Inventory Management:** Create, update, and organize menu categories and individual items along with pricing, descriptions, and thumbnails.
* **Table Configuration:** Define layout zones, table counts, and seating capacities.
* **Dynamic QR Generation:** Auto-generate unique QR codes mapped directly to specific tables.

### 2. Customer QR Menu
A seamless, mobile-optimized web interface that bypasses traditional paper menus:
* **Scan to Browse:** Customers scan a table-specific QR code to access the digital menu instantly without installing an app.
* **Order Customization:** Filter by category, select items, configure modifiers/preferences, and submit orders directly to the kitchen pipeline.

### 3. Kitchen Display System (KDS)
A real-time, high-efficiency interface for back-of-house kitchen operations:
* **Instant Order Routing:** Incoming orders from tables populate instantly on the kitchen monitor.
* **Status Management:** One-click status updates allowing chefs to transition orders smoothly through custom states (e.g., *New*, *Preparing*, *Ready*).

### 4. Live Order Tracking
* **Real-Time Status Updates:** Keeps customers updated on their phone screens regarding the precise progress of their order (*Preparing*, *Ready for Service*), drastically reducing customer anxiety and unneeded inquiries to staff.

### 5. Order Screen & Invoice Printing
* **Point of Service (POS) Station:** Dedicated employee view to monitor active table totals and handle billing.
* **Hardcopy Printing:** Instantly outputs professional invoices when connected to local printers.
* **Order Completion:** Mark accounts as closed and clear tables once payment processing is completed.

### 6. Feedback Management
* **Post-Meal Experience Survey:** Digital feedback screens prompt customers directly on completion of their meal to grade food quality, service, and atmosphere.

---

## 🎯 Secondary Features

* **In-Menu Mini Games:** Includes light interactive games (e.g., *Tic-Tac-Toe*) directly within the customer's digital menu layout to keep them engaged while waiting for their order.
* **Informative Public Website:** A public-facing web landing page outlining restaurant hours, location, contact coordinates, and integrated social media links.
* **Online Table Reservation:** Remote booking portal letting clients reserve specific tables or event slots for future dates from home.

---

## 📊 Application Interface Reference

### Administrative Panels & Workflow
* **Dashboard Overview:** Tracks key daily analytics like *Today's Revenue*, *Active Orders Count*, *Average Preparation Time*, and *Current Guest Volume* alongside a live order stream.
* **Category Controls:** Form structures to append custom sections (e.g., *Appetizers, Main Course, Desserts, Beverages*) with image uploads and visibility toggles.
* **Menu Item Adjustments:** Granular price assignment, description inputs, and item state management (Active / Inactive).
* **Table Management Matrix:** Track floor maps, seating capacities (e.g., Table 01 - 4 Persons), and localized restaurant regions (e.g., *Main Floor, VIP Lounge, Bar*).
* **Preferences Panel:** Global configuration controls for language mapping, default menu localizations, and base system currency flags (e.g., `USD ($)`).
