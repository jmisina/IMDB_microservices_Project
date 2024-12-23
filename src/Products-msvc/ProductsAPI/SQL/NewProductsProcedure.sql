CREATE OR REPLACE PROCEDURE get_new_products()
LANGUAGE plpgsql
AS $$
BEGIN
    PERFORM 1 FROM pg_class WHERE relname = 'temp_new_products';
    IF FOUND THEN
        EXECUTE 'DROP TABLE temp_new_products';
    END IF;

    CREATE TEMP TABLE temp_new_products AS
    SELECT data, mt_last_modified
    FROM mt_doc_product
    WHERE mt_last_modified >= NOW() - INTERVAL '7 days';

    RAISE NOTICE 'New products are available in temp_new_products table';
END;
$$;
