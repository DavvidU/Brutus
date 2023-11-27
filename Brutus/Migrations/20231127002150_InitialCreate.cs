using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brutus.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Konta",
                columns: table => new
                {
                    ID_Konta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SkrotHasla = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NrTelefonu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konta", x => x.ID_Konta);
                });

            migrationBuilder.CreateTable(
                name: "Przedmioty",
                columns: table => new
                {
                    ID_Przedmiotu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przedmioty", x => x.ID_Przedmiotu);
                });

            migrationBuilder.CreateTable(
                name: "Zalaczniki",
                columns: table => new
                {
                    ID_Zalacznika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dane = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zalaczniki", x => x.ID_Zalacznika);
                });

            migrationBuilder.CreateTable(
                name: "Admini",
                columns: table => new
                {
                    ID_Admina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admini", x => x.ID_Admina);
                    table.ForeignKey(
                        name: "FK_Admini_Konta_ID_Admina",
                        column: x => x.ID_Admina,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nauczyciele",
                columns: table => new
                {
                    ID_Nauczyciela = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nauczyciele", x => x.ID_Nauczyciela);
                    table.ForeignKey(
                        name: "FK_Nauczyciele_Konta_ID_Nauczyciela",
                        column: x => x.ID_Nauczyciela,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rodzice",
                columns: table => new
                {
                    ID_Rodzica = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rodzice", x => x.ID_Rodzica);
                    table.ForeignKey(
                        name: "FK_Rodzice_Konta_ID_Rodzica",
                        column: x => x.ID_Rodzica,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wiadomosci",
                columns: table => new
                {
                    ID_Wiadomosci = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Nadawcy = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wiadomosci", x => x.ID_Wiadomosci);
                    table.ForeignKey(
                        name: "FK_Wiadomosci_Konta_ID_Nadawcy",
                        column: x => x.ID_Nadawcy,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta");
                });

            migrationBuilder.CreateTable(
                name: "PrzedmiotyZalaczniki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Przedmiotu = table.Column<int>(type: "int", nullable: true),
                    ID_Zalacznika = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrzedmiotyZalaczniki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrzedmiotyZalaczniki_Przedmioty_ID_Przedmiotu",
                        column: x => x.ID_Przedmiotu,
                        principalTable: "Przedmioty",
                        principalColumn: "ID_Przedmiotu");
                    table.ForeignKey(
                        name: "FK_PrzedmiotyZalaczniki_Zalaczniki_ID_Zalacznika",
                        column: x => x.ID_Zalacznika,
                        principalTable: "Zalaczniki",
                        principalColumn: "ID_Zalacznika");
                });

            migrationBuilder.CreateTable(
                name: "NauczycielePrzedmioty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Nauczyciela = table.Column<int>(type: "int", nullable: true),
                    ID_Przedmiotu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NauczycielePrzedmioty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NauczycielePrzedmioty_Nauczyciele_ID_Nauczyciela",
                        column: x => x.ID_Nauczyciela,
                        principalTable: "Nauczyciele",
                        principalColumn: "ID_Nauczyciela");
                    table.ForeignKey(
                        name: "FK_NauczycielePrzedmioty_Przedmioty_ID_Przedmiotu",
                        column: x => x.ID_Przedmiotu,
                        principalTable: "Przedmioty",
                        principalColumn: "ID_Przedmiotu");
                });

            migrationBuilder.CreateTable(
                name: "Ogloszenia",
                columns: table => new
                {
                    ID_Ogloszenia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Nauczyciela = table.Column<int>(type: "int", nullable: true),
                    Tresc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogloszenia", x => x.ID_Ogloszenia);
                    table.ForeignKey(
                        name: "FK_Ogloszenia_Nauczyciele_ID_Nauczyciela",
                        column: x => x.ID_Nauczyciela,
                        principalTable: "Nauczyciele",
                        principalColumn: "ID_Nauczyciela");
                });

            migrationBuilder.CreateTable(
                name: "Testy",
                columns: table => new
                {
                    ID_Testu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzasTrwania = table.Column<TimeSpan>(type: "time", nullable: false),
                    LiczbaZadan = table.Column<int>(type: "int", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID_Nauczyciela = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testy", x => x.ID_Testu);
                    table.ForeignKey(
                        name: "FK_Testy_Nauczyciele_ID_Nauczyciela",
                        column: x => x.ID_Nauczyciela,
                        principalTable: "Nauczyciele",
                        principalColumn: "ID_Nauczyciela");
                });

            migrationBuilder.CreateTable(
                name: "Wychowawcy",
                columns: table => new
                {
                    ID_Wychowawcy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wychowawcy", x => x.ID_Wychowawcy);
                    table.ForeignKey(
                        name: "FK_Wychowawcy_Nauczyciele_ID_Wychowawcy",
                        column: x => x.ID_Wychowawcy,
                        principalTable: "Nauczyciele",
                        principalColumn: "ID_Nauczyciela",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KontaWiadomosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Odbiorcy = table.Column<int>(type: "int", nullable: true),
                    ID_Wiadomosci = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KontaWiadomosci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KontaWiadomosci_Konta_ID_Odbiorcy",
                        column: x => x.ID_Odbiorcy,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta");
                    table.ForeignKey(
                        name: "FK_KontaWiadomosci_Wiadomosci_ID_Wiadomosci",
                        column: x => x.ID_Wiadomosci,
                        principalTable: "Wiadomosci",
                        principalColumn: "ID_Wiadomosci");
                });

            migrationBuilder.CreateTable(
                name: "WiadomosciZalaczniki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Wiadomosci = table.Column<int>(type: "int", nullable: true),
                    ID_Zalacznika = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WiadomosciZalaczniki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WiadomosciZalaczniki_Wiadomosci_ID_Wiadomosci",
                        column: x => x.ID_Wiadomosci,
                        principalTable: "Wiadomosci",
                        principalColumn: "ID_Wiadomosci");
                    table.ForeignKey(
                        name: "FK_WiadomosciZalaczniki_Zalaczniki_ID_Zalacznika",
                        column: x => x.ID_Zalacznika,
                        principalTable: "Zalaczniki",
                        principalColumn: "ID_Zalacznika");
                });

            migrationBuilder.CreateTable(
                name: "Pytania",
                columns: table => new
                {
                    ID_Pytania = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Testu = table.Column<int>(type: "int", nullable: true),
                    Tresc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Punkty = table.Column<int>(type: "int", nullable: false),
                    WariantA = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WariantB = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WariantC = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WariantD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PoprawnaOdpowiedz = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pytania", x => x.ID_Pytania);
                    table.ForeignKey(
                        name: "FK_Pytania_Testy_ID_Testu",
                        column: x => x.ID_Testu,
                        principalTable: "Testy",
                        principalColumn: "ID_Testu");
                });

            migrationBuilder.CreateTable(
                name: "Klasy",
                columns: table => new
                {
                    ID_Klasy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerRoku = table.Column<int>(type: "int", nullable: false),
                    LiteraKlasy = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ID_Wychowawcy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasy", x => x.ID_Klasy);
                    table.ForeignKey(
                        name: "FK_Klasy_Wychowawcy_ID_Wychowawcy",
                        column: x => x.ID_Wychowawcy,
                        principalTable: "Wychowawcy",
                        principalColumn: "ID_Wychowawcy");
                });

            migrationBuilder.CreateTable(
                name: "KlasyPrzedmioty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Klasy = table.Column<int>(type: "int", nullable: true),
                    ID_Przedmiotu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlasyPrzedmioty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KlasyPrzedmioty_Klasy_ID_Klasy",
                        column: x => x.ID_Klasy,
                        principalTable: "Klasy",
                        principalColumn: "ID_Klasy");
                    table.ForeignKey(
                        name: "FK_KlasyPrzedmioty_Przedmioty_ID_Przedmiotu",
                        column: x => x.ID_Przedmiotu,
                        principalTable: "Przedmioty",
                        principalColumn: "ID_Przedmiotu");
                });

            migrationBuilder.CreateTable(
                name: "Uczniowie",
                columns: table => new
                {
                    ID_Ucznia = table.Column<int>(type: "int", nullable: false),
                    ID_Rodzica = table.Column<int>(type: "int", nullable: true),
                    ID_Klasy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uczniowie", x => x.ID_Ucznia);
                    table.ForeignKey(
                        name: "FK_Uczniowie_Klasy_ID_Klasy",
                        column: x => x.ID_Klasy,
                        principalTable: "Klasy",
                        principalColumn: "ID_Klasy");
                    table.ForeignKey(
                        name: "FK_Uczniowie_Konta_ID_Ucznia",
                        column: x => x.ID_Ucznia,
                        principalTable: "Konta",
                        principalColumn: "ID_Konta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uczniowie_Rodzice_ID_Rodzica",
                        column: x => x.ID_Rodzica,
                        principalTable: "Rodzice",
                        principalColumn: "ID_Rodzica");
                });

            migrationBuilder.CreateTable(
                name: "Oceny",
                columns: table => new
                {
                    ID_Oceny = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Waga = table.Column<int>(type: "int", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_Ucznia = table.Column<int>(type: "int", nullable: true),
                    ID_Nauczyciela = table.Column<int>(type: "int", nullable: true),
                    ID_Przedmiotu = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Wartosc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oceny", x => x.ID_Oceny);
                    table.ForeignKey(
                        name: "FK_Oceny_Przedmioty_ID_Przedmiotu",
                        column: x => x.ID_Przedmiotu,
                        principalTable: "Przedmioty",
                        principalColumn: "ID_Przedmiotu");
                    table.ForeignKey(
                        name: "FK_Oceny_Uczniowie_ID_Ucznia",
                        column: x => x.ID_Ucznia,
                        principalTable: "Uczniowie",
                        principalColumn: "ID_Ucznia");
                    table.ForeignKey(
                        name: "FK_Oceny_Wychowawcy_ID_Nauczyciela",
                        column: x => x.ID_Nauczyciela,
                        principalTable: "Wychowawcy",
                        principalColumn: "ID_Wychowawcy");
                });

            migrationBuilder.CreateTable(
                name: "UczniowieTesty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Ucznia = table.Column<int>(type: "int", nullable: true),
                    ID_Testu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UczniowieTesty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UczniowieTesty_Testy_ID_Testu",
                        column: x => x.ID_Testu,
                        principalTable: "Testy",
                        principalColumn: "ID_Testu");
                    table.ForeignKey(
                        name: "FK_UczniowieTesty_Uczniowie_ID_Ucznia",
                        column: x => x.ID_Ucznia,
                        principalTable: "Uczniowie",
                        principalColumn: "ID_Ucznia");
                });

            migrationBuilder.CreateTable(
                name: "UczniowieOceny",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Ucznia = table.Column<int>(type: "int", nullable: true),
                    ID_Oceny = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UczniowieOceny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UczniowieOceny_Oceny_ID_Oceny",
                        column: x => x.ID_Oceny,
                        principalTable: "Oceny",
                        principalColumn: "ID_Oceny");
                    table.ForeignKey(
                        name: "FK_UczniowieOceny_Uczniowie_ID_Ucznia",
                        column: x => x.ID_Ucznia,
                        principalTable: "Uczniowie",
                        principalColumn: "ID_Ucznia");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Klasy_ID_Wychowawcy",
                table: "Klasy",
                column: "ID_Wychowawcy");

            migrationBuilder.CreateIndex(
                name: "IX_KlasyPrzedmioty_ID_Klasy",
                table: "KlasyPrzedmioty",
                column: "ID_Klasy");

            migrationBuilder.CreateIndex(
                name: "IX_KlasyPrzedmioty_ID_Przedmiotu",
                table: "KlasyPrzedmioty",
                column: "ID_Przedmiotu");

            migrationBuilder.CreateIndex(
                name: "IX_KontaWiadomosci_ID_Odbiorcy",
                table: "KontaWiadomosci",
                column: "ID_Odbiorcy");

            migrationBuilder.CreateIndex(
                name: "IX_KontaWiadomosci_ID_Wiadomosci",
                table: "KontaWiadomosci",
                column: "ID_Wiadomosci");

            migrationBuilder.CreateIndex(
                name: "IX_NauczycielePrzedmioty_ID_Nauczyciela",
                table: "NauczycielePrzedmioty",
                column: "ID_Nauczyciela");

            migrationBuilder.CreateIndex(
                name: "IX_NauczycielePrzedmioty_ID_Przedmiotu",
                table: "NauczycielePrzedmioty",
                column: "ID_Przedmiotu");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_ID_Nauczyciela",
                table: "Oceny",
                column: "ID_Nauczyciela");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_ID_Przedmiotu",
                table: "Oceny",
                column: "ID_Przedmiotu");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_ID_Ucznia",
                table: "Oceny",
                column: "ID_Ucznia");

            migrationBuilder.CreateIndex(
                name: "IX_Ogloszenia_ID_Nauczyciela",
                table: "Ogloszenia",
                column: "ID_Nauczyciela");

            migrationBuilder.CreateIndex(
                name: "IX_PrzedmiotyZalaczniki_ID_Przedmiotu",
                table: "PrzedmiotyZalaczniki",
                column: "ID_Przedmiotu");

            migrationBuilder.CreateIndex(
                name: "IX_PrzedmiotyZalaczniki_ID_Zalacznika",
                table: "PrzedmiotyZalaczniki",
                column: "ID_Zalacznika");

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_ID_Testu",
                table: "Pytania",
                column: "ID_Testu");

            migrationBuilder.CreateIndex(
                name: "IX_Testy_ID_Nauczyciela",
                table: "Testy",
                column: "ID_Nauczyciela");

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_ID_Klasy",
                table: "Uczniowie",
                column: "ID_Klasy");

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_ID_Rodzica",
                table: "Uczniowie",
                column: "ID_Rodzica");

            migrationBuilder.CreateIndex(
                name: "IX_UczniowieOceny_ID_Oceny",
                table: "UczniowieOceny",
                column: "ID_Oceny");

            migrationBuilder.CreateIndex(
                name: "IX_UczniowieOceny_ID_Ucznia",
                table: "UczniowieOceny",
                column: "ID_Ucznia");

            migrationBuilder.CreateIndex(
                name: "IX_UczniowieTesty_ID_Testu",
                table: "UczniowieTesty",
                column: "ID_Testu");

            migrationBuilder.CreateIndex(
                name: "IX_UczniowieTesty_ID_Ucznia",
                table: "UczniowieTesty",
                column: "ID_Ucznia");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosci_ID_Nadawcy",
                table: "Wiadomosci",
                column: "ID_Nadawcy");

            migrationBuilder.CreateIndex(
                name: "IX_WiadomosciZalaczniki_ID_Wiadomosci",
                table: "WiadomosciZalaczniki",
                column: "ID_Wiadomosci");

            migrationBuilder.CreateIndex(
                name: "IX_WiadomosciZalaczniki_ID_Zalacznika",
                table: "WiadomosciZalaczniki",
                column: "ID_Zalacznika");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admini");

            migrationBuilder.DropTable(
                name: "KlasyPrzedmioty");

            migrationBuilder.DropTable(
                name: "KontaWiadomosci");

            migrationBuilder.DropTable(
                name: "NauczycielePrzedmioty");

            migrationBuilder.DropTable(
                name: "Ogloszenia");

            migrationBuilder.DropTable(
                name: "PrzedmiotyZalaczniki");

            migrationBuilder.DropTable(
                name: "Pytania");

            migrationBuilder.DropTable(
                name: "UczniowieOceny");

            migrationBuilder.DropTable(
                name: "UczniowieTesty");

            migrationBuilder.DropTable(
                name: "WiadomosciZalaczniki");

            migrationBuilder.DropTable(
                name: "Oceny");

            migrationBuilder.DropTable(
                name: "Testy");

            migrationBuilder.DropTable(
                name: "Wiadomosci");

            migrationBuilder.DropTable(
                name: "Zalaczniki");

            migrationBuilder.DropTable(
                name: "Przedmioty");

            migrationBuilder.DropTable(
                name: "Uczniowie");

            migrationBuilder.DropTable(
                name: "Klasy");

            migrationBuilder.DropTable(
                name: "Rodzice");

            migrationBuilder.DropTable(
                name: "Wychowawcy");

            migrationBuilder.DropTable(
                name: "Nauczyciele");

            migrationBuilder.DropTable(
                name: "Konta");
        }
    }
}
