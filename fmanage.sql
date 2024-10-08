-- Create the database
create database fmanage;

-- Use the database
use fmanage;

-- Create the users table with user_id as identity
create table users (
    user_id int primary key identity(1,1), 
    username varchar(50) not null unique,
    password varchar(255) not null,
    email varchar(100) not null unique
);

-- Create the expensecategories table
create table expensecategories (
    category_id int primary key,
    category_name varchar(50) not null unique
);

-- Create the expenses table with expense_id as identity
create table expenses (
    expense_id int primary key identity(1,1), 
    user_id int not null,
    amount int not null,
    category_id int not null,
    date date not null,
    description varchar(255),
    foreign key (user_id) references users(user_id),
    foreign key (category_id) references expensecategories(category_id)
);

-- Insert data into the users table
insert into users (username, password, email) values 
('rahulpuri', '12345', 'purirahul2002@gmail.com'),
('tanmaybhatt16', '78609', 'tbhatt@com'),
('suresh', '12345', 'suresh@gmail.com')


-- Insert data into the expensecategories table
insert into expensecategories (category_id, category_name) values 
(1, 'food'),
(2, 'transportation'),
(3, 'Utitlies');

-- Insert data into the expenses table
insert into expenses (user_id, amount, category_id, date, description) values 
(1, 100, 1, '2024-09-23', 'groceries'),
(2, 50, 2, '2024-09-24', 'bus ticket');

-- Select all users
select * from users;

-- Select all categories
select * from expensecategories;

-- Select all expenses
select * from expenses;

drop table users

ALTER TABLE expenses
ALTER COLUMN amount DECIMAL(10, 2);
SET IDENTITY_INSERT users ON;