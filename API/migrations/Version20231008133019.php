<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20231008133019 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SEQUENCE doctor_config_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE TABLE doctor_config (id INT NOT NULL, doctor_id INT NOT NULL, start_time TIME(0) WITHOUT TIME ZONE NOT NULL, end_time TIME(0) WITHOUT TIME ZONE NOT NULL, interval VARCHAR(10) NOT NULL, workdays TEXT NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_70161FF187F4FB17 ON doctor_config (doctor_id)');
        $this->addSql('COMMENT ON COLUMN doctor_config.workdays IS \'(DC2Type:simple_array)\'');
        $this->addSql('ALTER TABLE doctor_config ADD CONSTRAINT FK_70161FF187F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('DROP SEQUENCE doctor_config_id_seq CASCADE');
        $this->addSql('ALTER TABLE doctor_config DROP CONSTRAINT FK_70161FF187F4FB17');
        $this->addSql('DROP TABLE doctor_config');
    }
}
