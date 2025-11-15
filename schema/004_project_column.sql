CREATE TABLE user_data.project_column (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  project_id integer NOT NULL REFERENCES user_data.project (id) ON DELETE CASCADE,

  name varchar(32) NOT NULL,
  fg_color varchar(12) NULL,
  bg_color varchar(12) NULL,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon to read from project_column"
ON "user_data"."project_column"
AS PERMISSIVE
FOR SELECT
TO anon
USING (true);

CREATE POLICY "Allow anon to insert into project_column"
ON "user_data"."project_column"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

CREATE POLICY "Allow anon to update project_column"
ON "user_data"."project_column"
AS PERMISSIVE
FOR UPDATE
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."project_column"
FROM anon;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."project_column"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."project_column" ENABLE ROW LEVEL SECURITY;
