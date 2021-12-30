# Installatie
Volg deze instructies om de 'Fleet Management App' (Hierna FMA) te installeren.

### Minimum Systeem en software vereisten
- Visual Studio 2019
- Micosoft SQL Server (Express)
- Microsoft SQL Server Management Studio
- 4GB Geheugen of meer
- x64 CPU 2.0 Ghz of sneller
- Windows 10 TH1 1507 of nieuwer
- Windows Server 2016 of nieuwer

### TL;DR installatie 

1. Download de [github repo](https://github.com/Projectwerk-Fleet-Management/Fleet-Management-App/blob/main/FMA%20Client/SQLFmaDatabase.SQL)
2. Zet de database op via [deze](https://github.com/Projectwerk-Fleet-Management/Fleet-Management-App/blob/main/FMA%20Client/SQLFmaDatabase.SQL) query
3. Neem de connection string en plaats hem in de App.Config
4. Publish of run de code

## Codebase Downloaden

1. Ga naar de [github repo](https://github.com/Projectwerk-Fleet-Management/Fleet-Management-App/)
2. Klik op code
3. Download ZIP
4. Pak de zip uit


![Stappen!](https://cdn.discordapp.com/attachments/757966373485019136/926050748708560896/Screen_Shot_2021-12-30_at_10.54.25_AM.png "Voorbeeld stappen")

## Database Opzetten
Via Microsoft Server Management Studio voer deze [query](https://github.com/Projectwerk-Fleet-Management/Fleet-Management-App/blob/main/FMA%20Client/SQLFmaDatabase.SQL) uit om de databank aan maken. 

Bij het verbinden met uw server gelieve uw Server Name alvast bij te houden, u hebt deze later nodig. 

![](https://cdn.discordapp.com/attachments/757966373485019136/926054037261000754/Screen_Shot_2021-12-30_at_11.07.37_AM.png)

1. Klik op "New Query"
2. Plak de code uit de [github repo](https://github.com/Projectwerk-Fleet-Management/Fleet-Management-App/blob/main/FMA%20Client/SQLFmaDatabase.SQL)
3. Klik op "Execute"

De databank is aangemaakt.

![Stappen!](https://cdn.discordapp.com/attachments/757966373485019136/926040808237465620/Screen_Shot_2021-12-30_at_10.14.32_AM.png "Voorbeeld stappen")



## Database aan FMA koppelen
Open de codebase in Visual Studio

1. Ga naar de uitgepakte code die u heeft gedownload via Github
2. Klik op 'FMA Client'
3. Dubbelklik FMA Client.sln

In de solution explorer open alvast App.config door te dubbelklikken op het bestand.
Je kan de App.config terugvinden in het 'views project'.

![Stappen!](https://cdn.discordapp.com/attachments/757966373485019136/926052264605536266/Screen_Shot_2021-12-30_at_11.01.00_AM.png "Voorbeeld stappen")

Aan de linker kant van het scherm zou u normaal de server explorer teruvinden. 
Doe deze open en rechterklik op 'Data Connections', vervolgens op 'Add Connection...'

Bij Add Connection vult u uw server name in alsook de naam van de database (fmaDatabase) en klik op OK.

![](https://cdn.discordapp.com/attachments/757966373485019136/926054512400150599/Screen_Shot_2021-12-30_at_11.09.55_AM.png)

In de servermanager rechterklik op de net toegevoegde connection en vervolgens op properties.

![](https://media.discordapp.net/attachments/757966373485019136/926056824875135077/Screen_Shot_2021-12-30_at_11.19.07_AM.png)

in het propertiesvenster (normaal rechts onderaan) zie u connection string staan, kopier deze.

![](https://media.discordapp.net/attachments/757966373485019136/926056931012018196/Screen_Shot_2021-12-30_at_11.19.34_AM.png)

in de App.config file die u reeds heeft geopend vult u of verwisselt u de connection string in. 
![](https://media.discordapp.net/attachments/757966373485019136/926057567019487252/Screen_Shot_2021-12-30_at_11.22.04_AM.png)

## Application bouwen

Met 'Views' geselecteerd in het build menu klik op publish views
![](https://cdn.discordapp.com/attachments/588471677432758295/926083862919000074/Screen_Shot_2021-12-30_at_1.06.31_PM.png)

Selecteer Folder
![](https://media.discordapp.net/attachments/588471677432758295/926084327966670878/Screen_Shot_2021-12-30_at_1.07.57_PM.png)

Bij specific target kiest u weer voor folder
![](https://media.discordapp.net/attachments/588471677432758295/926084328180555786/Screen_Shot_2021-12-30_at_1.08.06_PM.png)

Indien u een andere locatie wilt mag u de folder locatie veranderen.
Hierna mag u op finish klikken.
![](https://media.discordapp.net/attachments/588471677432758295/926084328369295441/Screen_Shot_2021-12-30_at_1.08.17_PM.png)

Hierna resteert er enkel nog op 'Publish' geklikt te worden.
![](https://media.discordapp.net/attachments/588471677432758295/926084328558055444/Screen_Shot_2021-12-30_at_1.08.26_PM.png)

De applicatie zal builden en deze kan u terugvinden in de geselecteerde folder.
