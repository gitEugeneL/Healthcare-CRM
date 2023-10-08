<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20231008133255 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('ALTER TABLE doctor ADD doctor_config_id INT NOT NULL');
        $this->addSql('ALTER TABLE doctor ADD CONSTRAINT FK_1FC0F36AEBC96768 FOREIGN KEY (doctor_config_id) REFERENCES doctor_config (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_1FC0F36AEBC96768 ON doctor (doctor_config_id)');
        $this->addSql('ALTER TABLE doctor_config DROP CONSTRAINT fk_70161ff187f4fb17');
        $this->addSql('DROP INDEX uniq_70161ff187f4fb17');
        $this->addSql('ALTER TABLE doctor_config DROP doctor_id');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('ALTER TABLE doctor_config ADD doctor_id INT NOT NULL');
        $this->addSql('ALTER TABLE doctor_config ADD CONSTRAINT fk_70161ff187f4fb17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('CREATE UNIQUE INDEX uniq_70161ff187f4fb17 ON doctor_config (doctor_id)');
        $this->addSql('ALTER TABLE doctor DROP CONSTRAINT FK_1FC0F36AEBC96768');
        $this->addSql('DROP INDEX UNIQ_1FC0F36AEBC96768');
        $this->addSql('ALTER TABLE doctor DROP doctor_config_id');
    }
}
