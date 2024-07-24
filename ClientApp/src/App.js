import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
const user = "guest", message = "WooooW";

const App = () => {
    const [connection, setConnection] = useState([]);
    const [data, setData] = useState([]);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/dataHub",{
                            skipNegotiation: true,
                            transport: signalR.HttpTransportType.WebSockets
                          })
                          .configureLogging(signalR.LogLevel.Information)
                        .withAutomaticReconnect()
                        .build();

        connection.start().then(() => {
            connection.stream("Stream").subscribe({
                next: (item) => {
                    setData(prevData => [...prevData, item]);
                },
                complete: () => console.log("Stream completed"),
                error: (err) => console.error(err)
            });
        });

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div>
            <h1>Streaming Data</h1>
            <table>
                <thead>
                    <tr>
                        <th>Data</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, index) => (
                        <tr key={index}>
                            <td>{item.source_port}</td>
                            <td>{item.dest_port}</td>
                            <td>{item.source_ip}</td>
                            <td>{item.dest_ip}</td>
                            <td>{item.source_mac}</td>
                            <td>{item.dest_mac}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default App;




