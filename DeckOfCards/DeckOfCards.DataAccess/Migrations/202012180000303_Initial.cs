namespace DeckOfCards.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DeckId = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                        Suit = c.String(),
                        InDeck = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Decks", t => t.DeckId, cascadeDelete: true)
                .Index(t => t.DeckId);
            
            CreateTable(
                "dbo.Decks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DeckName = c.String(),
                        CardCount = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "DeckId", "dbo.Decks");
            DropIndex("dbo.Cards", new[] { "DeckId" });
            DropTable("dbo.Decks");
            DropTable("dbo.Cards");
        }
    }
}
