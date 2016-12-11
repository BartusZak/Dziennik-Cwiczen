# Dziennik-Cwiczen
Projekt na zaliczenie ćwiczeń z programowania obiektowego.

Autor: Bartłomiej "BartusZak" Płoszyński
www: bartuszak.pl

Niezrealizowane punkty:

1. Problem ze zdjęciem do danego ćwiczenia.
(Program nie wysyła pliku bitmapy do serwera MySql) - Dlaczego?

2. Problem z odsiwezeniem Activity,gdy wykonuję zapytania na DialogForm


Co udało się zrobić:

1. Konfiguracja MySql na VPS #Workbench #Linux #MySql
/etc/mysql/my.cnf
bind-address = 127.0.0.1
to
bind-address = 0.0.0.0
# mysql -u root -p
mysql> CREATE USER 'username'@'#' IDENTIFIED BY 'password';
-> GRANT ALL PRIVILEGES ON *.* TO 'username'@'#' WITH GRANT OPTION;
-> \q

2.  
