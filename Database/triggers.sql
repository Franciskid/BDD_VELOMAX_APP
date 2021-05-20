delimiter $$
#DROP TRIGGER if exists velomax.deletePiece$$
CREATE TRIGGER deletePiece BEFORE DELETE on fournisseurs
FOR EACH ROW
begin
    DELETE FROM pieces
    WHERE pieces.fournisseurId = old.siret;
end$$


#DROP TRIGGER if exists velomax.commandePieces$$
CREATE TRIGGER commandePieces after update
ON pieces 
for each row
Begin

if new.quantité = 0
then 
insert into Commandes(numCommande, clientid, pieceid, modeleid,quantité, dateCommande, dateLivraison)
values((FLOOR( 1 + RAND() * 99999999 )),1, new.idPiece,Null,10, now(), DATE_ADD(now(), interval (select delaiApprovisionnement from pieces where old.idPiece = idPiece) day));
end if;
end$$


#DROP event if exists velomax.AjoutPieceCommandee$$
CREATE 
    EVENT `AjoutPieceCommandee` 
    ON SCHEDULE EVERY 1 DAY STARTS now() 
    DO BEGIN    
	select * from commandes as com ;
    if com.clientid = 1 and day(com.dateLivraison) = day(now()) and com.modeleid = null
    then
    update velomax.pieces set pieces.quantité = com.quantité where pieces.idPiece = com.pieceid;
    end if;
    END$$

#DROP TRIGGER if exists velomax.EnleverStockCommande$$
CREATE TRIGGER EnleverStockCommande after insert
ON commandes 
for each row
Begin

if new.modeleid = null
then
update velomax.pieces set pieces.quantité = pieces.quantité - com.quantité where pieces.idPiece = com.pieceid;
end if;
end$$