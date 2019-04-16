## Generoija

Yksinkertainen C#-työkalu, jolla voidaan jäsentää Wiktionary-palvelun tarjoamia XML-tiedostoja. Vaatii [dotnet coren](https://dotnet.microsoft.com/download).

### Toiminnot

#### Listaa suomenkielisestä tiedostosta (fi.wiktionary) löytyvät otsikot

```powershell
dotnet run otsikot 2000 fiwiktionary-20190320-pages-articles-multistream.xml
```

#### Listaa suomenkielisestä tiedostosta (fi.wiktionary) löytyvät käännökset englanniksi

```powershell
dotnet run kaannokset 100 fiwiktionary-20190320-pages-articles-multistream.xml
```

#### Luo suomenkielisestä tiedostosta (fi.wiktionary) löytyvistä englannin käännöksistä JSON-tiedoston
Komento ton **teejson** sen jälkeen kuinka monta sanaa halutaan ottaa mukaan, sitten *fiwiktionary* -tiedosto, jonka jälkeen blocklist-sanoista, joita ei haluta ja viimeiseksi ulostulotiedoston nimi

Alla esimerkki
```powershell
dotnet run teejson 1000000 fiwiktionary-20190320-pages-articles-multistream.xml finnish-blocklist.txt tulos.json
```