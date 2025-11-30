CREATE TABLE user_data.soft_deletes (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  uid uuid NOT NULL DEFAULT gen_random_uuid(),
  user_id uuid NOT NULL REFERENCES auth.users (id),
  deleted timestamp DEFAULT CURRENT_TIMESTAMP,
  entity_type varchar(64) NOT NULL,
  parent_id int,

  PRIMARY KEY (id)
);

-- Create policies for allowing select, insert, update, and delete
CREATE POLICY "Allow anon/authenticated to read from soft_deletes"
ON "user_data"."soft_deletes"
AS PERMISSIVE
FOR SELECT
TO anon, authenticated
USING (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

CREATE POLICY "Allow anon/authenticated to insert into soft_deletes"
ON "user_data"."soft_deletes"
AS PERMISSIVE
FOR INSERT
TO anon, authenticated
WITH CHECK (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

CREATE POLICY "Allow anon/authenticated to update soft_deletes"
ON "user_data"."soft_deletes"
AS PERMISSIVE
FOR UPDATE
TO anon, authenticated
WITH CHECK (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

CREATE POLICY "Allow anon/authenticated to delete from soft_deletes"
ON "user_data"."soft_deletes"
AS PERMISSIVE
FOR DELETE
TO anon, authenticated
USING (
  (current_setting('request.jwt.claims', true)::json ->> 'sub') = user_id::text
);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE "user_data"."soft_deletes"
FROM anon, authenticated;

-- Grant select, insert, and update for anon
GRANT SELECT, INSERT, UPDATE, DELETE
ON TABLE "user_data"."soft_deletes"
TO anon, authenticated;

-- Enable RLS
ALTER TABLE "user_data"."soft_deletes" ENABLE ROW LEVEL SECURITY;