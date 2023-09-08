<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20230907175817 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('ALTER TABLE disease_doctor DROP CONSTRAINT fk_ab64352bd8355341');
        $this->addSql('ALTER TABLE disease_doctor DROP CONSTRAINT fk_ab64352b87f4fb17');
        $this->addSql('DROP TABLE disease_doctor');
        $this->addSql('ALTER TABLE disease ADD doctor_id INT DEFAULT NULL');
        $this->addSql('ALTER TABLE disease ADD CONSTRAINT FK_F3B6AC187F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('CREATE INDEX IDX_F3B6AC187F4FB17 ON disease (doctor_id)');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('CREATE TABLE disease_doctor (disease_id INT NOT NULL, doctor_id INT NOT NULL, PRIMARY KEY(disease_id, doctor_id))');
        $this->addSql('CREATE INDEX idx_ab64352b87f4fb17 ON disease_doctor (doctor_id)');
        $this->addSql('CREATE INDEX idx_ab64352bd8355341 ON disease_doctor (disease_id)');
        $this->addSql('ALTER TABLE disease_doctor ADD CONSTRAINT fk_ab64352bd8355341 FOREIGN KEY (disease_id) REFERENCES disease (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE disease_doctor ADD CONSTRAINT fk_ab64352b87f4fb17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE disease DROP CONSTRAINT FK_F3B6AC187F4FB17');
        $this->addSql('DROP INDEX IDX_F3B6AC187F4FB17');
        $this->addSql('ALTER TABLE disease DROP doctor_id');
    }
}
