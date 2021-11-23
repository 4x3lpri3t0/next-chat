// @ts-check
import React from "react";
import ChatList from "./components/ChatList";
import MessageList from "./components/MessageList";
import TypingArea from "./components/TypingArea";
import useChatHandlers from "./use-chat-handlers";

/**
 * @param {{
 *  onLogOut: () => void,
 *  onMessageSend: (message: string, roomId: string) => void,
 *  user: import("../../state").UserEntry
 * }} props
 */
export default function Chat({ onLogOut, user, onMessageSend }) {
    (async function () {
        // AXEL TODO
        //const res = await fetch('https://webchat-mockbot.azurewebsites.net/directline/token', { method: 'POST' });
        //const { token } = await res.json();

        const { ReactWebChat } = window.WebChat;

        window.ReactDOM.render(
            //<ReactWebChat directLine={window.WebChat.createDirectLine({ token })} />,

            <ReactWebChat directLine={window.WebChat.createDirectLine(
                {
                    secret: '',
                    token: '',
                    domain: 'http://127.0.0.1:3000/directline',
                    webSocket: false,
                    pollingInterval: 2000
                }

            )} />,

            document.getElementById('webchat')
        );

        document.querySelector('#webchat > *').focus();
    })().catch(err => console.error(err));

    const {
        onLoadMoreMessages,
        onUserClicked,
        message,
        setMessage,
        rooms,
        room,
        currentRoom,
        dispatch,
        messageListElement,
        roomId,
        messages,
        users,
    } = useChatHandlers(user);

    return (
        <div id="wrapper">

            <div id="left-chat" role="main" className="container py-5 px-4">
                <div className="chat-body row overflow-hidden shadow bg-light rounded flex-column">

                    <div className="py-2 bg-light">
                        <p className="h5 mb-0 py-1 chats-title">Chatbot</p>
                    </div>
                    <div id="webchat" role="main" className="messages-box flex flex-1"></div>
                </div>
            </div>

            <div id="right-chat" className="container py-5 px-4">
                <div className="chat-body row overflow-hidden shadow bg-light rounded">
                    <div className="col-4 px-0">
                        <ChatList
                            user={user}
                            onLogOut={onLogOut}
                            rooms={rooms}
                            currentRoom={currentRoom}
                            dispatch={dispatch}
                        />
                    </div>
                    {/* Chat Box*/}
                    <div className="col-8 px-0 flex-column bg-white rounded-lg">
                        <div className="px-4 py-4" style={{ borderBottom: "1px solid #eee" }}>
                            <h2 className="font-size-15 mb-0">{room ? room.name : "Room"}</h2>
                        </div>
                        <MessageList
                            messageListElement={messageListElement}
                            messages={messages}
                            room={room}
                            onLoadMoreMessages={onLoadMoreMessages}
                            user={user}
                            onUserClicked={onUserClicked}
                            users={users}
                        />

                        {/* Typing area */}
                        <TypingArea
                            message={message}
                            setMessage={setMessage}
                            onSubmit={(e) => {
                                e.preventDefault();
                                onMessageSend(message.trim(), roomId);
                                setMessage("");

                                messageListElement.current.scrollTop =
                                    messageListElement.current.scrollHeight;
                            }}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
}