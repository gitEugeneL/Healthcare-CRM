<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20231013174806 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SEQUENCE medical_record_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE TABLE medical_record (id INT NOT NULL, patient_id INT DEFAULT NULL, doctor_id INT DEFAULT NULL, specialization_id INT DEFAULT NULL, appointment_id INT NOT NULL, title VARCHAR(50) NOT NULL, description TEXT NOT NULL, doctor_note VARCHAR(100) DEFAULT NULL, created_at TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL, updated_at TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE INDEX IDX_F06A283E6B899279 ON medical_record (patient_id)');
        $this->addSql('CREATE INDEX IDX_F06A283E87F4FB17 ON medical_record (doctor_id)');
        $this->addSql('CREATE INDEX IDX_F06A283EFA846217 ON medical_record (specialization_id)');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_F06A283EE5B533F9 ON medical_record (appointment_id)');
        $this->addSql('COMMENT ON COLUMN medical_record.created_at IS \'(DC2Type:datetime_immutable)\'');
        $this->addSql('COMMENT ON COLUMN medical_record.updated_at IS \'(DC2Type:datetime_immutable)\'');
        $this->addSql('ALTER TABLE medical_record ADD CONSTRAINT FK_F06A283E6B899279 FOREIGN KEY (patient_id) REFERENCES patient (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE medical_record ADD CONSTRAINT FK_F06A283E87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE medical_record ADD CONSTRAINT FK_F06A283EFA846217 FOREIGN KEY (specialization_id) REFERENCES specialization (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE medical_record ADD CONSTRAINT FK_F06A283EE5B533F9 FOREIGN KEY (appointment_id) REFERENCES appointment (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE appointment ADD medical_record_id INT DEFAULT NULL');
        $this->addSql('ALTER TABLE appointment ADD CONSTRAINT FK_FE38F844B88E2BB6 FOREIGN KEY (medical_record_id) REFERENCES medical_record (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_FE38F844B88E2BB6 ON appointment (medical_record_id)');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('ALTER TABLE appointment DROP CONSTRAINT FK_FE38F844B88E2BB6');
        $this->addSql('DROP SEQUENCE medical_record_id_seq CASCADE');
        $this->addSql('ALTER TABLE medical_record DROP CONSTRAINT FK_F06A283E6B899279');
        $this->addSql('ALTER TABLE medical_record DROP CONSTRAINT FK_F06A283E87F4FB17');
        $this->addSql('ALTER TABLE medical_record DROP CONSTRAINT FK_F06A283EFA846217');
        $this->addSql('ALTER TABLE medical_record DROP CONSTRAINT FK_F06A283EE5B533F9');
        $this->addSql('DROP TABLE medical_record');
        $this->addSql('DROP INDEX UNIQ_FE38F844B88E2BB6');
        $this->addSql('ALTER TABLE appointment DROP medical_record_id');
    }
}
