CREATE TABLE IF NOT EXISTS site_data.site_log (
  id integer NOT NULL GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1),
  created timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  severity varchar(8) NOT NULL,
  message varchar(2048),
  context_data jsonb NULL,
  client_ip varchar(42) NULL,

  PRIMARY KEY (id)
);

-- Create policy for allowing insert
CREATE POLICY "Allow anon to write to site_log"
ON "site_data"."site_log"
AS PERMISSIVE
FOR INSERT
TO anon
WITH CHECK (true);

-- Remove default privileges
REVOKE ALL PRIVILEGES
ON TABLE site_data.site_log
FROM anon;

-- Grant insert for anon
GRANT INSERT
ON TABLE site_data.site_log
TO anon;

-- Enable RLS
ALTER TABLE "site_data"."site_log" ENABLE ROW LEVEL SECURITY;
