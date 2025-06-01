namespace eShop.Ordering.Infrastructure.EntityConfigurations;

class BuyerEntityTypeConfiguration
    : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> buyerConfiguration)
    {
        buyerConfiguration.ToTable("buyers");

        buyerConfiguration.Property(b => b.Id)
            .UseHiLo("buyerseq");

        buyerConfiguration.Property(b => b.IdentityGuid)
            .HasMaxLength(200);

        buyerConfiguration.HasIndex("IdentityGuid")
            .IsUnique(true);

        buyerConfiguration.HasIndex("IdentityGuid")
            .IsUnique(true);

        buyerConfiguration.Property(b => b.Name);

        buyerConfiguration.HasMany(b => b.PaymentMethods)
            .WithOne(pm => pm.Buyer)
            .OnDelete(DeleteBehavior.Cascade);

        buyerConfiguration.HasMany(b => b.Orders)
            .WithOne(o => o.Buyer)
            .HasForeignKey(o => o.BuyerId);
    }
}
