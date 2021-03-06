CREATE DATABASE IF NOT EXISTS velomax;
CREATE USER if not exists 'bozo'@'localhost' IDENTIFIED BY 'bozo';
GRANT SELECT, SHOW VIEW ON velomax.* TO 'bozo'@'localhost';

use velomax;

#drop table if exists Commandes;
#drop table if exists Assemblages;
#drop table if exists Pieces;
#drop table if exists Modeles;
#drop table if exists Comptes;
#drop table if exists Fournisseurs;
#drop table if exists Clients;
#drop table if exists Adresse;
#drop table if exists Fidelio;

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
    delaidelivraison varchar(30),
    
    foreign key (idAdresse) references Adresse(idAdresse) on delete cascade
);

create table if not exists Pieces
(
	idPiece varchar(10) primary key not null,
    nom varchar(30),
    fournisseurId int not null default 17254121,
    numProduit int not null default 1,
    prix float not null default 1,
    quantité int not null default 1,
    dateIntroduction datetime,
    dateDiscontinuation datetime,
    delaiApprovisionnement int default 10,
    
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
    panier  varchar(10)
    
);
ALTER TABLE assemblages AUTO_INCREMENT = 101;

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
    nom varchar(100),
    prenom varchar(100),
    idAdresse int,
    telephone varchar(100),
    courriel varchar(100),
    nomContact varchar(100),
    remise int,
    fidelio bool,
    idFidelio int,
    dateAdhesionFidelio datetime,
    
    foreign key (idFidelio) references Fidelio(idFidelio),
    foreign key (idAdresse) references Adresse(idAdresse)  on delete cascade
);
ALTER TABLE Modeles AUTO_INCREMENT = 101;


create table if not exists Commandes
(
	idCommande int primary key  auto_increment not null, 
    numCommande int,
    clientid int,
    pieceid varchar(30),
    modeleid int,
    quantité int default 1,
    dateCommande datetime,
    dateLivraison dateTime,
    
    foreign key (clientid) references Clients(idClient) on delete cascade
    #foreign key (pieceid) references Pieces(idPiece)
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
("rue quelque part","paris","75018","france"),
("rue des lisiées","paris","75004","france"),
("rue des lisiées","marseille","36250","france"),
("rue des lisiées","paris","75018","france"),
("rue des lisiées","lyon","69004","france"),
("rue des lisiées","lille","45021","france"),
("rue des lisiées","montpellier","36502","france"),
("rue des lisiées","grenoble","87025","france"),
("rue des lisiées","grenoble","87025","france"),
("place de la Bourse","toulon","54001","france");

insert into fournisseurs(siret, nom, contact, idAdresse, score,delaidelivraison) 
values(17524121, "Michelin", "George Mickeal", 21, 1,3),
(17261521, "Aqualand", "Mickeal Jackson", 22, 3,5),
(87512761, "Dunlop", "Buzz Aldrin", 23, 2,2),
(17254121, "L'Elysée", "Quelqu'un", 24, 2, 10);

insert into Pieces(idPiece, nom, fournisseurId, quantité,prix) 
values('C32', 'Cadre', 17524121, 10,11),
('C34', 'Cadre', 17524121, 12,10),
('C76', 'Cadre', 17524121, 2,32),
('C43',  'Cadre', 17261521, 4,21),
('C44f',  'Cadre', 17261521, 12,33),
('C43f',  'Cadre', 17261521, 12,29),
('C01' , 'Cadre', 17261521, 12,2),
('C02' , 'Cadre', 17261521, 1,8),
('C15',  'Cadre', 87512761, 12,11),
('C87' , 'Cadre', 87512761, 0,17),
('C87f',  'Cadre', 87512761, 12,18),
('C25' , 'Cadre', 87512761, 32,20),
('C26' , 'Cadre', 17254121, 12,21);
 
insert into Pieces(nom, idPiece, quantité,prix) 
values('Guidon', 'G7', 1,23),
('Guidon', 'G9', 10,15),
('Guidon', 'G12', 4,21);

insert into Pieces(nom, idPiece,quantité,prix) 
values('Freins', 'F3',8,27),
('Freins', 'F9',7,43);

insert into Pieces(nom, idPiece,quantité,prix) 
values('Selle', 'S88',10,23),
('Selle', 'S37',10,21),
('Selle', 'S35',10,16),
('Selle', 'S02',10,27),
('Selle', 'S03',15,18),
('Selle', 'S36',19,34),
('Selle', 'S87',16,45),
('Selle', 'S34',10,17);

insert into Pieces(nom, idPiece,quantité,prix) 
values('Dérailleur avant', 'DV133',12,21),
('Dérailleur avant', 'DV17',17,12),
('Dérailleur avant', 'DV87',32,12),
('Dérailleur avant', 'DV57',15,21),
('Dérailleur avant', 'DV15',91,18),
('Dérailleur avant', 'DV41',33,31),
('Dérailleur avant', 'DV132',21,25);

insert into Pieces(nom, idPiece,quantité,prix) 
values('Pédalier', 'P12',9,41),
('Pédalier', 'P34',7,53),
('Pédalier', 'P1',5,21),
('Pédalier', 'P15',8,45),
('Ordinateur', 'O2',4,21),
('Ordinateur', 'O4',3,0),
('Panier', 'S01',2,12),
('Panier', 'S05',8,23),
('Panier', 'S74',7,95),
('Panier', 'S73',3,41),
('Réflecteur', 'R02',6,12),
('Réflecteur', 'R09',9,42),
('Réflecteur', 'R10',7,35),
('Roue', 'R45',3,15),
('Roue', 'R48',6,41),
('Roue', 'R12',8,25),
('Roue', 'R19',9,32),
('Roue', 'R1',8,56),
('Roue', 'R11',5,12),
('Roue', 'R46',8,255),
('Roue', 'R47',8,24),
('Roue', 'R32',5,63),
('Roue', 'R18',7,12),
('Roue', 'R2',9,78),
('Roue', 'R44',4,21),
('Dérailleur avant', 'DR56',2,3),
('Dérailleur arrière', 'DR87',5,12),
('Dérailleur arrière', 'DR86',7,56),
('Dérailleur arrière', 'DR23',5,32),
('Dérailleur arrière', 'DR76',2,14),
('Dérailleur arrière', 'DR52',3,21);





insert into fidelio(nom,prix, duree_annee, rabais)
values('Fidélio', 15, 1, 5),
('Fidélio Or', 25, 1, 8),
('Fidélio Platine', 60, 1, 10),
('Fidélio Max', 100, 2, 12);

insert into assemblages(nom, grandeur, cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier)
values('Kilimandjaro', 'Adultes', 'C32', 'G7', 'F3', 'S88', 'DV133', 'DR56', 'R45', 'R46', Null,  'P12', 'O2', Null),
('NorthPole', 'Adultes', 'C34', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R48', 'R47', Null, 'P12', Null, Null),
('MontBlanc', 'Jeunes', 'C76', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R45', 'R47', Null, 'P12', 'O2', Null),
('Hooligan', 'Jeunes', 'C76', 'G7', 'F3', 'S88', 'DV87', 'DR86', 'R12', 'R32', Null,  'P12', Null, Null),
('Orléans', 'Hommes', 'C43', 'G9', 'F9', 'S37', 'DV57', 'DR86', 'R19', 'R18', 'R02',  'P34', Null, Null),
('Orléans', 'Dames', 'C44f', 'G9', 'F9', 'S35', 'DV57', 'DR86', 'R19', 'R18', 'R02',  'P34', Null, Null),
('BlueJay', 'Hommes', 'C43', 'G9', 'F9', 'S37', 'DV57', 'DR87', 'R19', 'R18', 'R02',  'P34', 'O4', Null),
('BlueJay', 'Dames', 'C43f', 'G9', 'F9', 'S35', 'DV57', 'DR87', 'R19', 'R18', 'R02',  'P34', 'O4', Null),
('Trail_Explorer', 'Filles', 'C01', 'G12', Null, 'S02', Null, Null, 'R1', 'R2', 'R09',  'P1', Null, 'S01'),
('Trail_Explorer', 'Garcons', 'C02', 'G12', Null, 'S03', Null, Null, 'R1', 'R2', 'R09',  'P1', Null, 'S05'),
('Night_Hawk', 'Jeunes', 'C15', 'G12', 'F9', 'S36', 'DV15', 'DR23', 'R11', 'R12', 'R10', 'P15', Null, 'S74'),
('Tierra_Verde', 'Hommes', 'C87', 'G12', 'F9', 'S36', 'DV41', 'DR76', 'R11', 'R12', 'R10',  'P15', Null, 'S74'),
('Tierra_Verde', 'Dames', 'C87f', 'G12', 'F9', 'S34', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', Null, 'S73'),
('Mud_Zinger_I', 'Jeunes', 'C25', 'G7', 'F3', 'S87', 'DV132', 'DR52', 'R44', 'R47', Null, 'P12', Null, Null),
('Mud_Zinger_II', 'Adultes', 'C26', 'G7', 'F3', 'S87', 'DV133', 'DR52', 'R44', 'R47', Null, 'P12', Null, Null);


insert into modeles(nom, prix, ligne)
values('Kilimandjaro',  569, 'VTT'),
('NorthPole',  329, 'VTT'),
('MontBlanc',  399, 'VTT'),
('Hooligan',  199, 'VTT'),
('Orléans',  229, 'Vélo de course'),
('Orléans',  229, 'Vélo de course'),
('BlueJay', 349, 'Vélo de course'),
('BlueJay',  349, 'Vélo de course'),
('Trail_Explorer',  129, 'Classique'),
('Trail_Explorer',  129, 'Classique'),
('Night_Hawk',  189, 'Classique'),
('Tierra_Verde',  199, 'Classique'),
('Tierra_Verde',  199, 'Classique'),
('Mud_Zinger_I',  279, 'BMX'),
('Mud_Zinger_II',  359, 'BMX');

insert into Clients(typeClient, nom, idAdresse, telephone, courriel, nomContact, remise)
values('boutique',"VELOMAX",1,"01 85 63 82 36","velomax.noreply@gmail.com","Legrand", 0),
('boutique',"Haribo",2,"06 26 43 43 14","hario@gmail.com","Defer", 1),
('boutique',"EDF",3,"06 46 45 41 24","EDF@gmail.com","Dureau", 10),
('boutique',"Micheline",4,"06 56 42 28 08","PMU@gmail.com","Blanc", 5), 
('boutique',"PMU",5,"06 36 15 22 18","PMU@gmail.com","Blanc", 8);

insert into Clients(typeClient,prenom,nom,idAdresse,telephone,courriel,nomContact,fidelio,idFidelio,dateAdhesionFidelio)
values('individuel',"Jean","Raoul",6,"06 26 22 18 40","j.roul@gmail.com","rara",True,1,"20/08/13"),
('individuel',"Louise","Pril",7,"06 46 52 18 41","l.pril@gmail.com","rara",True,3,"20/08/13"),
('individuel',"Yanis","Quille",8,"06 16 50 98 44","y.quille@gmail.com","rara",True,2,"21/02/14"),
('individuel',"Nabile","zoul",9,"06 06 51 38 34","n.zoul@gmail.com","rara",True,3,"20/08/13"),
('individuel',"Marc","desbois",10,"06 52 45 15 23","marcusmorrisfan@gmail.com","rara",True,3,"20/07/22"),
('individuel',"Thomas","felin",11,"06 87 54 36 41","thothoaimelamoto@gmail.com","rara",True,2,"20/07/23"),
('individuel',"Anas","leroy",12,"06 87 56 52 12","Anasleroi@gmail.com","rara",True,3,"20/06/13"),
('individuel',"Delphine","dufresne",13,"06 45 62 35 45","dufresnedel@gmail.com","rara",True,3,"20/08/14"),
('individuel',"Valentine","duhamel",14,"06 45 87 51 23","vavadutar@gmail.com","rara",True,2,"20/08/13"),
('individuel',"Claude","banta",15,"06 74 83 16 45","bantanavidaloca@gmail.com","rara",True,3,"21/02/13"),
('individuel',"Clémence","leveille",16,"06 46 13 56 98","clémence.l@gmail.com","rara",False ,null,null),
('individuel',"Dominique","gilbert",17,"06 82 19 73 56","domidesdom@gmail.com","rara",True,2,"20/08/13"),
('individuel',"Frédérique","lebrun",18,"06 42 51 85 63","frédo.dug@gmail.com","rara",True,3,"20/06/13"),
('individuel',"Léon","Quirion",19,"06 42 15 86 32","léonquirion@gmail.com","rara",True,3,"20/06/13"),
('individuel',"Christian","Villeneuve",20,"06 12 54 81 67","christistheone@gmail.com","rara",True,2,"20/06/13");

SET SQL_SAFE_UPDATES = 0;

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
(8,6,Null,102,"10/03/12","10/03/12"),
(8,6,Null,103,"10/03/12","10/03/12"),
(9,7,null,104,"12/03/12","18/03/12"),
(10,8,null,105,"15/03/12","20/03/12"),
(11,9,null,106,"19/03/12","25/03/12"),
(12,10,null,107,"21/03/12","28/03/12"),
(12,10,null,108,"12/03/12","18/03/12"),
(13,11,null,109,"25/03/12","30/03/12"),
(14,12,null,110,"01/04/12","03/04/12"),
(15,13,null,111,"25/03/12","30/03/12"),
(16,14,null,112,"25/03/12","30/03/12"),
(16,14,'DR23',null,"25/03/12","30/03/12"),
(17,15,null,113,"25/03/12","30/03/12"),
(17,15,null,114,"25/03/12","30/03/12"),
(18,16,'R18',null,"25/03/12","30/03/12"),
(18,16,'R19',null,"25/03/12","30/03/12"),
(18,16,'R2',null,"25/03/12","30/03/12"),
(19,17,null,115,"25/03/12","30/03/12"),
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

    