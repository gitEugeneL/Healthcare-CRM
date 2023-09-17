<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20230917162830 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE TABLE doctor_disease (doctor_id INT NOT NULL, disease_id INT NOT NULL, PRIMARY KEY(doctor_id, disease_id))');
        $this->addSql('CREATE INDEX IDX_9C96096C87F4FB17 ON doctor_disease (doctor_id)');
        $this->addSql('CREATE INDEX IDX_9C96096CD8355341 ON doctor_disease (disease_id)');
        $this->addSql('ALTER TABLE doctor_disease ADD CONSTRAINT FK_9C96096C87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_disease ADD CONSTRAINT FK_9C96096CD8355341 FOREIGN KEY (disease_id) REFERENCES disease (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE disease DROP CONSTRAINT fk_f3b6ac187f4fb17');
        $this->addSql('DROP INDEX idx_f3b6ac187f4fb17');
        $this->addSql('ALTER TABLE disease DROP doctor_id');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('ALTER TABLE doctor_disease DROP CONSTRAINT FK_9C96096C87F4FB17');
        $this->addSql('ALTER TABLE doctor_disease DROP CONSTRAINT FK_9C96096CD8355341');
        $this->addSql('DROP TABLE doctor_disease');
        $this->addSql('ALTER TABLE disease ADD doctor_id INT DEFAULT NULL');
        $this->addSql('ALTER TABLE disease ADD CONSTRAINT fk_f3b6ac187f4fb17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('CREATE INDEX idx_f3b6ac187f4fb17 ON disease (doctor_id)');
    }
}
