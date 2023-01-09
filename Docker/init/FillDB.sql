\c inventory_db;

INSERT INTO product(name, price, available_quantity) VALUES ('Avatar', 15.99, 5);
INSERT INTO product(name, price, available_quantity) VALUES ('Jame Bond Skyfall', 15.99, 10);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und der Stein der Weisen', 10.99, 2);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und die Kammer des Schreckens', 12.99, 3);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und der Gefangene von Askaban', 12.99, 4);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und der Feuerkelch', 14.99, 2);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und der Orden des Phoenix', 17.99, 12);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und der Halbblutprinz', 9.99, 8);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und die Heiligtuemer des Todes Teil 1', 20.00, 7);
INSERT INTO product(name, price, available_quantity) VALUES ('Harry Potter und die Heiligtuemer des Todes Teil 2', 18.90, 9);
INSERT INTO product(name, price, available_quantity) VALUES ('Tom und Jerry Sammelbox', 55.90, 13);
INSERT INTO product(name, price, available_quantity) VALUES ('DeadPool', 13.99, 6);
INSERT INTO product(name, price, available_quantity) VALUES ('DeadPool 2', 16.99, 7);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 1', 20.99, 1);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 2', 20.99, 7);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 3', 20.99, 3);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 4', 20.99, 6);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 5', 20.99, 1);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 6', 20.99, 20);
INSERT INTO product(name, price, available_quantity) VALUES ('Orange is the new Black Staffel 7', 20.99, 10);
INSERT INTO product(name, price, available_quantity) VALUES ('Fast & Furious Sammelbox', 70.00, 3);


\c orders_db;

INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'user1', 1, 4, 64.96);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 3, 1, 10.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 4, 1, 12.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 5, 1, 12.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 6, 1, 14.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 7, 1, 17.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 8, 1, 19.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 9, 1, 20.00);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'filmLover23', 10, 1, 18.90);

INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'tom&jerryisbest', 11, 10, 559.90);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'fastbutnotfurious', 21, 1, 70.00);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'user2', 12, 2, 27.98);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'user2', 13, 1, 16.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'Orange', 14, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'Orange', 15, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'Orange', 16, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'OrangeNewBlack', 16, 2, 41.98);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'OrangeNewBlack', 17, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'OrangeNewBlack', 18, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'OrangeNewBlack', 19, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'OrangeNewBlack', 20, 1, 20.99);
INSERT INTO orders(order_date, username, product_id, quantity, total) VALUES (NOW(), 'JamesBond123', 2, 9, 143.91);
