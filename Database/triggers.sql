delimiter $$
DROP TRIGGER if exists velomax.deletePiece$$
CREATE TRIGGER deletePiece BEFORE DELETE on fournisseurs
FOR EACH ROW
begin
    DELETE FROM pieces
    WHERE pieces.fournisseurId = old.siret;
end$$


DROP TRIGGER if exists velomax.commandePieces$$
CREATE TRIGGER commandePieces after update
ON pieces 
for each row
Begin

if new.quantité = 0
then 
insert into Commandes(numCommande, clientid, pieceid, modeleid,quantité, dateCommande, dateLivraison)
values(rand(),1, new.idPiece,Null,10, now(), now() + (select delaiApprovisionnement from pieces where old.idPiece = idPiece));
end if;
end$$


DROP TRIGGER if exists velomax.AjoutPieceCommandee$$
CREATE TRIGGER AjoutPieceCommandee after update
ON pieces 
for each row
Begin

if new.quantité = 0
then 
insert into Commandes(numCommande, clientid, pieceid, modeleid,quantité, dateCommande, dateLivraison)
values(rand(),1, new.idPiece,Null,10, now(), now() + (select delaiApprovisionnement from pieces where old.idPiece = idPiece));
end if;
end$$

DROP TRIGGER if exists velomax.EnleverStockCommande$$
CREATE TRIGGER EnleverStockCommande after insert
ON commandes 
for each row
Begin

insert into Commandes(numCommande, clientid, pieceid, modeleid,quantité, dateCommande, dateLivraison)
values(rand(),1, new.idPiece,Null,10, now(), now() + (select delaiApprovisionnement from pieces where old.idPiece = idPiece));

end$$