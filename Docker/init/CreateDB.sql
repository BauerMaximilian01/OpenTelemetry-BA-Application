CREATE DATABASE inventory_db;

\c inventory_db;

CREATE TABLE product (
    product_id SERIAL,
    name VARCHAR(150),
    price DECIMAL(15, 2),
    available_quantity INT,
    CONSTRAINT pk_product PRIMARY KEY(product_id)
);

CREATE DATABASE orders_db;

\c orders_db;

CREATE TABLE orders (
    order_id SERIAL,
    order_date TIMESTAMP,
    username VARCHAR(50),
    product_id INT,
    quantity INT,
    total DECIMAL(15, 2),
    CONSTRAINT pk_order PRIMARY KEY(order_id)
);