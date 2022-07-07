import React from 'react';
import Form from "@rjsf/core";

function MyForm() {
    const [schema, setSchema] = React.useState({});

    React.useEffect(() => {
        const fetchData = async () => {
            const res = await fetch('/User/GetModelSchema');
            const json = await res.json();
            setSchema(json);
        };
        fetchData();
    }, []);

    const onSubmit = async ({formData}, e) => {       
        console.log("Data submitting: ",  formData);
        const response = await fetch('/User/Submit', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(formData) });
        const json = await response.json();
        console.log("Su8nmit Response: ",  json);             
    }

    const log = (type) => console.log.bind(console, type);

return (
    <Form schema={schema}
        onChange={log("changed")}
        onSubmit={onSubmit}
        onError={log("errors")} />
)}

export default MyForm;