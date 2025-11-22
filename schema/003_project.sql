CREATE TABLE user_data.projects (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  created timestamp DEFAULT CURRENT_TIMESTAMP,
  updated timestamp DEFAULT CURRENT_TIMESTAMP,
  is_deleted boolean DEFAULT false,
  user_id uuid NOT NULL REFERENCES auth.users (id),
  
  title varchar(200) NOT NULL,
  description varchar(600) NULL,
  default_card_fg_color varchar(12) NULL,
  default_card_bg_color varchar(12) NULL,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, and update
CREATE POLICY "Allow anon to read from projects"
ON "user_data"."projects"
AS PERMISSIVE
FOR SELECT
TO anon
USING (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

CREATE POLICY "Allow anon to insert into projects"
ON "user_data"."projects"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

CREATE POLICY "Allow anon to update projects"
ON "user_data"."projects"
AS PERMISSIVE
FOR UPDATE
TO anon
WITH CHECK (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."projects"
FROM anon;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE
ON TABLE "user_data"."projects"
TO anon;

-- Enable RLS
ALTER TABLE "user_data"."projects" ENABLE ROW LEVEL SECURITY;
