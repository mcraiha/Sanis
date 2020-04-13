## Generoija

Yksinkertainen C#-työkalu, jolla voidaan jäsentää Wiktionary-palvelun tarjoamia XML-tiedostoja. Vaatii [dotnet coren](https://dotnet.microsoft.com/download).  

Suomenkieliset alkuperäistiedostot löytyvät https://dumps.wikimedia.org/fiwiktionary/  
Englanninkieliset alkuperäistiedostot löytyvät https://dumps.wikimedia.org/enwiktionary/

### Toiminnot suomekieliselle materiaalille

#### Listaa suomenkielisestä tiedostosta (fi.wiktionary) löytyvät otsikot
Komento on **otsikot**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, ja viimeiseksi *fiwiktionary* -tiedosto

Alla esimerkki
```powershell
dotnet run otsikot 2000 fiwiktionary-20190320-pages-articles-multistream.xml
```

#### Listaa suomenkielisestä tiedostosta (fi.wiktionary) löytyvät käännökset englanniksi
Komento on **kaannokset**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, seuraavaksi etsittävä käännösteksti, ja viimeiseksi *fiwiktionary* -tiedosto

Alla esimerkki
```powershell
dotnet run kaannokset 100 *englanti: fiwiktionary-20190320-pages-articles-multistream.xml
```

#### Luo suomenkielisestä tiedostosta (fi.wiktionary) löytyvistä englannin käännöksistä JSON-tiedoston
Komento on **teejson**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, seuraavaksi etsittävä käännösteksti, sitten *fiwiktionary* -tiedosto, jonka jälkeen blocklist-sanoista, joita ei haluta ja viimeiseksi ulostulotiedoston nimi

Alla esimerkki
```powershell
dotnet run teejson 1000000 *englanti: fiwiktionary-20190320-pages-articles-multistream.xml finnish-blocklist.txt tulos.json
```

### Toiminnot englanninkieliselle materiaalille

#### Listaa englanninkielisestä tiedostosta (en.wiktionary) löytyvät otsikot
Komento on **otsikot**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, ja viimeiseksi *enwiktionary* -tiedosto

Alla esimerkki
```powershell
dotnet run otsikot 2000 enwiktionary-20190320-pages-articles-multistream.xml
```

#### Listaa englanninkielisestä tiedostosta (en.wiktionary) löytyvät käännökset suomeksi
Komento on **kaannokset**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, seuraavaksi etsittävä käännösteksti, ja viimeiseksi *enwiktionary* -tiedosto

Alla esimerkki
```powershell
dotnet run kaannokset 100 "* Finnish:" enwiktionary-20190320-pages-articles-multistream.xml
```

#### Luo englanninkielisestä tiedostosta (en.wiktionary) löytyvistä suomen käännöksistä JSON-tiedoston
Komento on **teejson**, sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, seuraavaksi etsittävä käännösteksti, sitten *enwiktionary* -tiedosto, jonka jälkeen blocklist-sanoista, joita ei haluta ja viimeiseksi ulostulotiedoston nimi

Alla esimerkki
```powershell
dotnet run teejson 1000000 "* Finnish:" enwiktionary-20190320-pages-articles-multistream.xml english-blocklist.txt tulos.json
```

### Suomenkielisen ja englanninkielisen tiedoston mehustaminen

Mehustamisella tarkoitetaan linkkitietojen lisäämistä sanojen yhteyteen

Alla esimerkki
```powershell
dotnet run mehusta suomi.json english.json suomi_mehustettu.json english_mehustettu.json
```


### Tulosten pakkaaminen

Jos halutaan pakata vaikkapa tulos.json-tiedosto hyödyntäen Zstandard-toteutusta, olisi komento seuraavanlainen Windowsilla
```powershell
zstd.exe -19 tulos.json -o tulos.zst
```
