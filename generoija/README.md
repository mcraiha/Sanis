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

```powershell
dotnet run teejson 100 fiwiktionary-20190320-pages-articles-multistream.xml tulos.json
```