CREATE DATABASE IF NOT EXISTS velomax;
CREATE USER if not exists 'bozo'@'localhost' IDENTIFIED BY 'bozo';
GRANT SELECT, SHOW VIEW ON velomax.* TO 'bozo'@'localhost';

use velomax;

drop table if exists Commandes;
drop table if exists Assemblages;
drop table if exists Pieces;
drop table if exists Modeles;
drop table if exists Comptes;
drop table if exists Fournisseurs;
drop table if exists Clients;
drop table if exists Adresse;
drop table if exists Fidelio;

create table if not exists Adresse
(
	idAdresse int primary key auto_increment not null,
    rue varchar(30),
    ville varchar(30),
    codePostal varchar(30),
    pays varchar(30)
);

create table if not exists Fournisseurs
(
	siret int primary key not null,
    nom varchar(30),
    contact varchar(30),
    idAdresse int,
    score enum('1', '2', '3', '4'),
    
    foreign key (idAdresse) references Adresse(idAdresse)
);

create table if not exists Pieces
(
	idPiece varchar(10) primary key not null,
    nom varchar(30),
    fournisseurId int not null default 0,
    numProduit int not null default 1,
    prix float not null default 1,
    quantité int not null default 0,
    dateIntroduction datetime,
    dateDiscontinuation datetime,
    delaiApprovisionnement datetime,
    
	foreign key (fournisseurId) references fournisseurs(siret)
);

create table if not exists Assemblages
(
	idAssemblage int primary key auto_increment not null,
	nom varchar(30),
    grandeur ENUM('Hommes', 'Dames', 'Adultes', 'Jeunes', 'Garçons', 'Filles'),
    cadre  varchar(10) ,
    guidon  varchar(10),
    freins  varchar(10) ,
	selle varchar(10) ,
    derailleur_avant  varchar(10) ,
    derailleur_arriere  varchar(10) ,
    roue_avant  varchar(10) ,
    roue_arriere varchar(10) ,
    reflecteurs varchar(10) ,
    pedalier  varchar(10) ,
    ordinateur varchar(10) ,
    panier  varchar(10) ,
    
	foreign key (cadre) references Pieces(idPiece),
    foreign key (guidon) references Pieces(idPiece),
    foreign key (freins) references Pieces(idPiece),
    foreign key (selle) references Pieces(idPiece),
    foreign key (derailleur_avant) references Pieces(idPiece),
    foreign key (derailleur_arriere) references Pieces(idPiece),
    foreign key (roue_avant) references Pieces(idPiece),
    foreign key (roue_arriere) references Pieces(idPiece),
    foreign key (reflecteurs) references Pieces(idPiece),
    foreign key (pedalier) references Pieces(idPiece),
    foreign key (ordinateur) references Pieces(idPiece),
    foreign key (panier) references Pieces(idPiece)
   
);
ALTER TABLE assemblages AUTO_INCREMENT = 100001;

create table if not exists Modeles
(
	idModele INT PRIMARY KEY auto_increment NOT NULL,
    nom varchar(30),
    prix int,
    ligne varchar(30),
    dateIntroduction datetime default CURRENT_TIMESTAMP,
    dateDiscontinuation datetime default CURRENT_TIMESTAMP
);
ALTER TABLE Modeles AUTO_INCREMENT = 101;

create table if not exists Fidelio
(
	idFidelio INT auto_increment PRIMARY KEY NOT NULL,
    nom varchar(30),
    prix int,
    duree_annee float4,
    rabais float4
);


create table if not exists Clients
(
	idClient int primary key auto_increment not null,
    typeClient enum('individuel', 'boutique') not null,
    nom varchar(30),
    prenom varchar(30),
    idAdresse int,
    telephone varchar(30),
    courriel varchar(30),
    nomContact varchar(30),
    remise int,
    fidelio bool,
    idFidelio int,
    dateAdhesionFidelio datetime,
    
    foreign key (idFidelio) references Fidelio(idFidelio),
    foreign key (idAdresse) references Adresse(idAdresse)
);
ALTER TABLE Modeles AUTO_INCREMENT = 101;


create table if not exists Commandes
(
	idCommande int primary key  auto_increment not null, 
    numCommande int,
    clientid int,
    pieceid varchar(30),
    modeleid int,
    dateCommande datetime,
    dateLivraison dateTime,
    
    foreign key (clientid) references Clients(idClient),
    foreign key (pieceid) references Pieces(idPiece)
    #foreign key (modeleid) references modeles(idModele)
    
);

create table if not exists Comptes
(
	idCompte int primary key auto_increment not null,
    pseudo varchar(30),
    motdepasse varchar(255) default null,
    unique key(pseudo)
);

insert into Comptes(pseudo, motdepasse) values ('root', sha1('rootroot')), ('bozo', sha1('bozobozo'));

insert into adresse(idAdresse, rue, ville, codePostal, pays)
values(1, "rue nulle part", "Marseille", "13000", "France");

insert into fournisseurs(siret, nom, contact, idAdresse, score) 
values(0, "TEST", "CONTACT_TEST", 1, 2);

insert into Pieces(idPiece, nom, fournisseurId) 
values('C32', 'Cadre', 0),
('C34', 'Cadre', 0),
('C76', 'Cadre', 0),
('C43',  'Cadre', 0),
('C44f',  'Cadre', 0),
('C43f',  'Cadre', 0),
('C01' , 'Cadre', 0),
('C02' , 'Cadre', 0),
('C15',  'Cadre', 0),
('C87' , 'Cadre', 0),
('C87f',  'Cadre', 0),
('C25' , 'Cadre', 0),
('C26' , 'Cadre', 0);
 
insert into Pieces(nom, idPiece) 
values('Guidon', 'G7'),
('Guidon', 'G9'),
('Guidon', 'G12');

insert into Pieces(nom, idPiece) 
values('Freins', 'F3'),
('Freins', 'F9');

insert into Pieces(nom, idPiece) 
values('Selle', 'S88'),
('Selle', 'S37'),
('Selle', 'S35'),
('Selle', 'S02'),
('Selle', 'S03'),
('Selle', 'S36'),
('Selle', 'S87'),
('Selle', 'S34');

insert into Pieces(nom, idPiece) 
values('Dérailleur avant', 'DV133'),
('Dérailleur avant', 'DV17'),
('Dérailleur avant', 'DV87'),
('Dérailleur avant', 'DV57'),
('Dérailleur avant', 'DV15'),
('Dérailleur avant', 'DV41'),
('Dérailleur avant', 'DV132');

insert into Pieces(nom, idPiece) 
values('Dérailleur avant', 'DR56'),
('Dérailleur arrière', 'DR87'),
('Dérailleur arrière', 'DR86'),
('Dérailleur arrière', 'DR23'),
('Dérailleur arrière', 'DR76'),
('Dérailleur arrière', 'DR52');


insert into Pieces(nom, idPiece) 
values('Roue', 'R45'),
('Roue', 'R48'),
('Roue', 'R12'),
('Roue', 'R19'),
('Roue', 'R1'),
('Roue', 'R11'),
('Roue', 'R46'),
('Roue', 'R47'),
('Roue', 'R32'),
('Roue', 'R18'),
('Roue', 'R2'),
('Roue', 'R44');


insert into Pieces(nom, idPiece) 
values('Réflecteur', 'R02'),
('Réflecteur', 'R09'),
('Réflecteur', 'R10');

insert into Pieces(nom, idPiece) 
values('Pédalier', 'P12'),
('Pédalier', 'P34'),
('Pédalier', 'P1'),
('Pédalier', 'P15');

insert into Pieces(nom, idPiece) 
values('Ordinateur', 'O2'),
('Ordinateur', 'O4');


insert into Pieces(nom, idPiece) 
values('Panier', 'S01'),
('Panier', 'S05'),
('Panier', 'S74'),
('Panier', 'S73');


insert into fidelio(nom,prix, duree_annee, rabais)
values('Fidélio', 15, 1, 5),
('Fidélio Or', 25, 2, 8),
('Fidélio Platine', 60, 2, 10),
('Fidélio Max', 100, 3, 12);

insert into assemblages(nom, grandeur, cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier)
values('Kilimandjaro', 'Adultes', 'C32', 'G7', 'F3', 'S88', 'DV133', 'DR56', 'R45', 'R46', Null,  'P12', 'O2', Null),
('NorthPole', 'Adultes', 'C34', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R48', 'R47', Null, 'P12', Null, Null),
('MontBlanc', 'Jeunes', 'C76', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R45', 'R47', Null, 'P12', 'O2', Null),
('Hooligan', 'Jeunes', 'C76', 'G7', 'F3', 'S88', 'DV87', 'DR86', 'R12', 'R32', Null,  'P12', Null, Null),
('Orléans', 'Hommes', 'C43', 'G9', 'F9', 'S37', 'DV57', 'DR86', 'R19', 'R18', 'R02',  'P34', Null, Null),
('Orléans', 'Dames', 'C44f', 'G9', 'F9', 'S35', 'DV57', 'DR86', 'R19', 'R18', 'R02',  'P34', Null, Null),
('BlueJay', 'Hommes', 'C43', 'G9', 'F9', 'S37', 'DV57', 'DR87', 'R19', 'R18', 'R02',  'P34', 'O4', Null),
('BlueJay', 'Dames', 'C43f', 'G9', 'F9', 'S35', 'DV57', 'DR87', 'R19', 'R18', 'R02',  'P34', 'O4', Null),
('Trail Explorer', 'Filles', 'C01', 'G12', Null, 'S02', Null, Null, 'R1', 'R2', 'R09',  'P1', Null, 'S01'),
('Trail Explorer', 'Garcons', 'C02', 'G12', Null, 'S03', Null, Null, 'R1', 'R2', 'R09',  'P1', Null, 'S05'),
('Night Hawk', 'Jeunes', 'C15', 'G12', 'F9', 'S36', 'DV15', 'DR23', 'R11', 'R12', 'R10', 'P15', Null, 'S74'),
('Tierra Verde', 'Hommes', 'C87', 'G12', 'F9', 'S36', 'DV41', 'DR76', 'R11', 'R12', 'R10',  'P15', Null, 'S74'),
('Tierra Verde', 'Dames', 'C87f', 'G12', 'F9', 'S34', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', Null, 'S73'),
('Mud Zinger I', 'Jeunes', 'C25', 'G7', 'F3', 'S87', 'DV132', 'DR52', 'R44', 'R47', Null, 'P12', Null, Null),
('Mud Zinger II', 'Adultes', 'C26', 'G7', 'F3', 'S87', 'DV133', 'DR52', 'R44', 'R47', Null, 'P12', Null, Null);


insert into modeles(nom, prix, ligne)
values('Kilimandjaro',  569, 'VTT'),
('NorthPole',  329, 'VTT'),
('MontBlanc',  399, 'VTT'),
('Hooligan',  199, 'VTT'),
('Orléans',  229, 'Vélo de course'),
('Orléans',  229, 'Vélo de course'),
('BlueJay', 349, 'Vélo de course'),
('BlueJay',  349, 'Vélo de course'),
('Trail Explorer',  129, 'Classique'),
('Trail Explorer',  129, 'Classique'),
('Night Hawk',  189, 'Classique'),
('Tierra Verde',  199, 'Classique'),
('Tierra Verde',  199, 'Classique'),
('Mud Zinger I',  279, 'BMX'),
('Mud Zinger II',  359, 'BMX');

insert into adresse( rue, ville, codePostal, pays)
values("rue de la pompe","paris","75002","france"),
("Boulevard de Belleville","paris","75020","france"),
("rue Lecourbe","paris","75019","france"),
("Rue de Vaugirard","paris","75012","france"),
("rue de Courcelles","paris","75014","france"),
("avenue de la République","paris","75016","france"),
("Avenue Mozart","paris","75018","france"),
("Avenue des jardins","paris","75018","france"),
("rue des tulipes","Sarcelles","95","france"),
("place des clichys","paris","75018","france"),
("rue des croyances","paris","75018","france"),
("boulevard Charles de Gaulle","paris","75018","france"),
("rue de Vichy","paris","75018","france"),
("Allée du baron souriant","paris","75018","france"),
("rue des lisiées","paris","75018","france"),
("place de la Bourse","paris","75010","france");

insert into Clients(typeClient, nom, idAdresse, telephone, courriel, nomContact)
values('boutique',"Haribo",1,"06 26 43 43 14","hario@gmail.com","Defer"),
('boutique',"EDF",2,"06 46 45 41 24","EDF@gmail.com","Dureau"),
('boutique',"PMU",4,"06 36 15 22 18","PMU@gmail.com","Blanc"),
('boutique',"Micheline",3,"06 56 42 28 08","PMU@gmail.com","Blanc");

insert into Clients(typeClient,prenom,nom,idAdresse,telephone,courriel,nomContact,remise,fidelio,idFidelio,dateAdhesionFidelio)
values('individuel',"Jean","Raoul",1,"06 26 22 18 40","j.roul@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Louise","Pril",1,"06 46 52 18 41","l.pril@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Yanis","Quille",1,"06 16 50 98 44","y.quille@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Nabile","zoul",1,"06 06 51 38 34","n.zoul@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Marc","desbois",1,"06 52 45 15 23","marcusmorrisfan@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Thomas","felin",1,"06 87 54 36 41","thothoaimelamoto@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Anas","leroy",1,"06 87 56 52 12","Anasleroi@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Delphine","dufresne",1,"06 45 62 35 45","dufresnedel@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Valentine","duhamel",1,"06 45 87 51 23","vavadutar@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Claude","banta",1,"06 74 83 16 45","bantanavidaloca@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Clémence","leveille",1,"06 46 13 56 98","clémence.l@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Dominique","gilbert",1,"06 82 19 73 56","domidesdom@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Frédérique","lebrun",1,"06 42 51 85 63","frédo.dug@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Léon","Quirion",1,"06 42 15 86 32","léonquirion@gmail.com","rara",1,2,3,"20/06/13"),
('individuel',"Christian","Villeneuve",1,"06 12 54 81 67","christistheone@gmail.com","rara",1,2,3,"20/06/13");


insert into Commandes(numCommande, clientid, pieceid, modeleid, dateCommande, dateLivraison)
values(1,1,'C01',Null,"17/01/12","20/01/12"),
(1,1,'G12',Null,"17/01/12","20/01/12"),
(2,2,'S37',Null,"10/02/12","20/02/12"),
(2,2,'S88',Null,"10/02/12","20/02/12"),
(2,2,'S88',Null,"10/02/12","20/02/12"),
(3,3,'S01',Null,"19/02/12","24/02/12"),
(4,2,'C43f',Null,"25/02/12","27/02/12"),
(5,1,'C25',Null,"25/02/12","27/02/12"),
(6,4,'C15',Null,"01/03/12","07/03/12"),
(6,4,'C26',Null,"01/03/12","07/03/12"),
(6,4,'S73',Null,"01/03/12","07/03/12"),
(7,5,Null,101,"03/03/12","05/03/12"),
(8,6,Null,101,"10/03/12","10/03/12"),
(8,6,Null,102,"10/03/12","10/03/12"),
(9,7,null,102,"12/03/12","18/03/12"),
(10,8,null,103,"15/03/12","20/03/12"),
(11,9,null,103,"19/03/12","25/03/12"),
(12,10,null,104,"21/03/12","28/03/12"),
(12,10,null,104,"12/03/12","18/03/12"),
(13,11,null,104,"25/03/12","30/03/12"),
(14,12,null,105,"01/04/12","03/04/12"),
(15,13,null,107,"25/03/12","30/03/12");