CREATE OR REPLACE FUNCTION get_new_products()
RETURNS TABLE (data JSONB, mt_last_modified TIMESTAMP WITH TIME ZONE)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT data, mt_last_modified
    FROM mt_doc_product
    WHERE mt_last_modified >= NOW() - INTERVAL '7 days';
END;
$$;
