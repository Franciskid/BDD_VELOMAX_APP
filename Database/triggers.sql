delimiter $$

CREATE TRIGGER deletePiece BEFORE DELETE on fournisseurs
FOR EACH ROW
begin
    DELETE FROM pieces
    WHERE pieces.fournisseurId = old.siret;
end$$