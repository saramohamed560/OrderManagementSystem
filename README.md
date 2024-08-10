# OrderManagement.API
System that allow customers to place orders, view their order history, and allow administrators to manage orders , Invoices.
# Key Features:
* User Management: Manage user accounts, authentication, and authorization with secure JWT-based authentication and role-based access control.
* Product Catalog Management: Create, read, update, and delete products with detailed information including name, description, price, stock availability.
* Order Management: Handle order creation, order status tracking .
* Admin Capabilities: Provided administrators with tools to manage products, and access all order and invoice information.
* Payment Integration: Integrate with Stripe payment gateway to facilitate secure online payments, supporting various payment methods.
* Caching: Utilized Redis as an in-memory database to cache responses for frequently requested data, improving application performance and reducing database load.
# Technologies & Tools
* .NET 8.0 - C#
* SQLServer database
* Entity Framwork Core
* LINQ
* Identity Package
* Onion Archeticture pattern
* Unit Of Work and Generic Repository pattern
* RESTful API
* Specification Design pattern
* AutoMapper
* JWT
* Redis for In-memory database
* Integrate with Stripe payment gateway
* RBAC(Role-Based-Access-Control).
