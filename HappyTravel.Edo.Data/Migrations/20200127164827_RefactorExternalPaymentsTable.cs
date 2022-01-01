using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class RefactorExternalPaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ExternalPayments",
                schema: null,
                newName: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.RenameColumn(
                name: "CreditCardId",
                table: "Payments",
                newName: "AccountId",
                schema: null);

            // All exists payments is credit cards
            migrationBuilder.Sql("update \"Payments\" set \"PaymentMethod\" = 2");

            // Actualize payment statuses
            migrationBuilder.Sql(@"update ""Payments""
                set ""Status"" = CASE
                    WHEN ""Bookings"".""PaymentStatus"" = 2 THEN 4
                    WHEN ""Bookings"".""PaymentStatus"" = 4 THEN 6
                    ELSE ""Payments"".""Status"" 
                END
                FROM ""Bookings""
                WHERE ""Payments"".""BookingId"" = ""Bookings"".""Id"" and ""Bookings"".""PaymentStatus"" in (2, 4)");

            // Entities for account payments
            migrationBuilder.Sql(@"INSERT INTO ""Payments""
            (""BookingId"", ""Created"", ""Modified"", ""AccountId"", ""AccountNumber"", ""Amount"", ""Currency"", ""PaymentMethod"", ""Status"", ""Data"")
            select 
               b.""Id"" as ""BookingId"",
               b.""Created"",
               b.""Created"" as ""Modified"",
               l.""AccountId"",
               l.""AccountId"" as ""AccountNumber"",
               l.""Amount"",
               case a.""Currency""
                    when 0 then 'NotSpecified'
                    when 1 then 'USD'
                    when 2 then 'EUR'
                    when 3 then 'AED'
                    when 4 then 'SAR'
               end as ""Currency"",
               b.""PaymentMethod"",
               case b.""PaymentStatus""
                   when 1 then 1
                   when 2 then 4
                   when 4 then 6
                   when 5 then 1
               end as ""Status"",
               jsonb_build_object('customerIp', '::1')  as ""Data""
            from ""Bookings"" b
            inner join (select ab.""ReferenceCode"", ab.""AccountId"", sum(ab.""Amount"") as ""Amount""
                from ""AccountBalanceAuditLogs"" ab
                where ab.""Type"" = 3 group by ab.""AccountId"", ab.""ReferenceCode"") l on l.""ReferenceCode"" = b.""ReferenceCode""
            inner join ""PaymentAccounts"" a on a.""Id"" = l.""AccountId""
            where b.""PaymentMethod"" = 1 and b.""PaymentStatus"" in (1, 2, 4, 5);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update ""Payments""
                set ""Status"" = CASE
                    WHEN ""Status"" = 4 THEN 1
                    WHEN ""Status"" = 6 THEN 1
                    ELSE ""Status"" 
                END
                WHERE ""Status"" in (4, 6)");

            migrationBuilder.Sql(@"delete from ""Payments"" where ""PaymentMethod"" = 1");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments",
                schema: null);

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Payments",
                newName: "CreditCardId",
                schema: null);

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: null,
                newName: "ExternalPayments");
        }
    }
}
