-- 06.03. 2019
-- za potrebe Rdeèega kartona se je menjalo ime dveh stolpcev
USE KVPOdeloTest;  
GO  
EXEC sp_rename 'KVPDocument.predlog_izboljsave2' , 'AktivnostRK', 'COLUMN';
EXEC sp_rename 'KVPDocument.PredlogPopravila' , 'OpisNapakeRK', 'COLUMN';

alter table KVPDocument alter column AktivnostRK varchar(5000);
alter table KVPDocument alter column OpisNapakeRK varchar(5000);

GO


use KVPOdeloTest
go
alter table Nastavitve
add StevilkaRK int

go

use KVPOdeloTest
go
alter table KVPDocument
add VarnostRK bit,
DatumPoslanePosteZaVarnost datetime

alter table Users
add PozarniReferent bit
go

/*28.3.2019*/
alter table KVPSkupina
add Aktivnost bit

/*2.4.2019*/
alter table Users
add SecondRoleID int null,
constraint FK_SecondRole
foreign key (SecondRoleID) references Vloga (VlogaID)

alter table KVPDocument
add RKVnesel int null,
constraint FK_RKVNESEL_USER
foreign key (RKVnesel) references Users (Id)

/* 03.09.2019 */
alter table Izplacila add SyncEmployee bit, SyncDate datetime

  