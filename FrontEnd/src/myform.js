import React from 'react';
import Form from "@rjsf/core";

function MyForm() {
    const [schema, setSchema] = React.useState({});

    React.useEffect(() => {
        const fetchData = async () => {
            const res = await fetch('https://localhost:7120/Admin/GetPrimitiveSchema');
            const json = await res.json();
            setSchema(json);
        };
        fetchData();
    });

    const log = (type) => console.log.bind(console, type);

return (
    <Form schema={schema}
        onChange={log("changed")}
        onSubmit={log("submitted")}
        onError={log("errors")} />
)}

export default MyForm;