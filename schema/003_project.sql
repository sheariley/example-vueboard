CREATE TABLE user_data.project (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  user_id integer NOT NULL REFERENCES auth.users (id),
  
  title varchar(200) NOT NULL,
  description varchar(600) NULL,
  default_card_fg_color varchar(12) NULL,
  default_card_bg_color varchar(12) NULL,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon to read from project"
ON "user_data"."project"
AS PERMISSIVE
FOR SELECT
TO anon
USING (true);

CREATE POLICY "Allow anon to insert into project"
ON "user_data"."project"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

CREATE POLICY "Allow anon to update project"
ON "user_data"."project"
AS PERMISSIVE
FOR UPDATE
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."project"
FROM anon;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."project"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."project" ENABLE ROW LEVEL SECURITY;
