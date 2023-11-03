<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20231103132802 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('DROP SEQUENCE office_id_seq CASCADE');
        $this->addSql('CREATE SEQUENCE office_number_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('DROP INDEX uniq_74516b0296901f54');
        $this->addSql('ALTER TABLE office DROP CONSTRAINT office_pkey');
        $this->addSql('ALTER TABLE office DROP id');
        $this->addSql('ALTER TABLE office ADD PRIMARY KEY (number)');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('DROP SEQUENCE office_number_seq CASCADE');
        $this->addSql('CREATE SEQUENCE office_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('DROP INDEX office_pkey');
        $this->addSql('ALTER TABLE office ADD id INT NOT NULL');
        $this->addSql('CREATE UNIQUE INDEX uniq_74516b0296901f54 ON office (number)');
        $this->addSql('ALTER TABLE office ADD PRIMARY KEY (id)');
    }
}
