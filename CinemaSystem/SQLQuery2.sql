insert into actors (Name, MainImg) values ('Pasta (Linguine)', '5.png');
insert into actors (Name, MainImg) values ('Outdoor Mosquito Repellent Lantern', '7.png');
insert into actors (Name, MainImg) values ('Magnetic Whiteboard', '6.png');
insert into actors (Name, MainImg) values ('Microwave Popcorn Maker', '7.png');
insert into actors (Name, MainImg) values ('Pet Grooming Glove', '1.png');
insert into actors (Name, MainImg) values ('Smart Home Security Camera', '6.png');
insert into actors (Name, MainImg) values ('Car Windshield Sun Shade', '3.png');
insert into actors (Name, MainImg) values ('Chicken Breasts', '2.png');
insert into actors (Name, MainImg) values ('Spinach and Feta Stuffed Chicken Breast', '9.png');

INSERT INTO Categories (Name, Description) VALUES
('Action', 'Movies with intense physical activity and stunts'),
('Comedy', 'Movies intended to make the audience laugh'),
('Drama', 'Serious stories with emotional themes'),
('Horror', 'Movies designed to scare and thrill viewers'),
('Romance', 'Love stories focusing on relationships and emotions'),
('Sci-Fi', 'Science fiction movies about futuristic concepts'),
('Adventure', 'Exciting stories with exploration or quests'),
('Animation', 'Movies created using animated techniques'),
('Documentary', 'Non-fictional movies presenting factual information');

iNSERT INTO Cinemas (Name, Location, ImageUrl) VALUES
('Cinema One', 'Cairo', '1.png'),
('Cinema Two', 'Alexandria', '2.png'),
('Cinema Three', 'Giza', '3.png'),
('Cinema Four', 'Mansoura', '4.png'),
('Cinema Five', 'Tanta', '5.png'),
('Cinema Six', 'Aswan', '6.png'),
('Cinema Seven', 'Ismailia', '7.png'),
('Cinema Eight', 'Suez', '8.png'),
('Cinema Nine', 'Marsa Matrouh', '9.png');

INSERT INTO Movies (Name, Description, Status, [Date], [Time], MainImg, SubImg, ActorId, CinemaId, CategoryId)
VALUES
('Movie One', 'Action-packed adventure film', 1, '2025-01-01', '18:00', '1.png', '1.png', 1, 1, 1),
('Movie Two', 'Romantic comedy about love and fate', 1, '2025-02-01', '19:00', '2.png', '2.png', 2, 2, 2),
('Movie Three', 'Science fiction with futuristic themes', 1, '2025-03-01', '20:00', '3.png', '3.png', 3, 3, 3),
('Movie Four', 'Thrilling horror experience', 0, '2025-04-01', '21:00', '4.png', '4.png', 4, 4, 4),
('Movie Five', 'Emotional drama with strong characters', 1, '2025-05-01', '17:00', '5.png', '5.png', 5, 5, 5),
('Movie Six', 'Animated movie for all ages', 1, '2025-06-01', '16:30', '6.png', '6.png', 6, 6, 6),
('Movie Seven', 'Epic fantasy adventure', 1, '2025-07-01', '20:30', '7.png', '7.png', 7, 7, 7),
('Movie Eight', 'Documentary about nature and wildlife', 1, '2025-08-01', '15:00', '8.png', '8.png', 8, 8, 8),
('Movie Nine', 'Classic comedy film', 1, '2025-09-01', '19:30', '9.png', '9.png', 9, 9, 9);
select * from movies
select * from categories
select * from actors
select * from cinemas