CREATE OR REPLACE FUNCTION public.create_site_log_entry(
  severity varchar(8),
  message varchar(2048),
  context_data jsonb,
  client_ip varchar(42) DEFAULT NULL
) RETURNS BOOLEAN
LANGUAGE plpgsql
SET search_path = ''
SECURITY INVOKER
AS $$
BEGIN
  INSERT INTO site_data.site_log
  ("severity", "message", "context_data", "client_ip")
  VALUES
  (severity, message, context_data, client_ip);
  RETURN TRUE;
END;
$$;
