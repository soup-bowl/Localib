import { useContext, useState } from "react"
import { useHistory } from "react-router-dom"
import {
	IonModal,
	IonHeader,
	IonToolbar,
	IonButtons,
	IonButton,
	IonTitle,
	IonContent,
	IonItem,
	IonInput,
	IonNote,
	IonList,
	IonInputPasswordToggle,
} from "@ionic/react"
import { useQueryClient } from "@tanstack/react-query"
import { UserContext } from "../context/UserContext"

interface Props {
	open: boolean
	onClose: () => void
	onSave: () => void
}

const Settings: React.FC<Props> = ({ open, onClose, onSave }) => {
	const queryClient = useQueryClient()
	const userContext = useContext(UserContext)
	const history = useHistory()

	if (!userContext) {
		throw new Error("SettingsPanel must be used within a UserProvider")
	}

	const { username, setUsername, password, setPassword } = userContext
	const [newUsername, setNewUsername] = useState<string>(username)
	const [newPassword, setNewPassword] = useState<string>(password)

	const handleSave = () => {
		console.log("Save", newUsername, newPassword, username, password)
		setUsername(newUsername)
		setPassword(newPassword)
		queryClient.invalidateQueries()
		onSave()
	}

	return (
		<IonModal isOpen={open}>
			<IonHeader>
				<IonToolbar>
					<IonButtons slot="start">
						<IonButton onClick={() => onClose()}>Close</IonButton>
					</IonButtons>
					<IonTitle>Settings</IonTitle>
					<IonButtons slot="end">
						<IonButton strong={true} onClick={handleSave}>
							Save
						</IonButton>
					</IonButtons>
				</IonToolbar>
			</IonHeader>
			<IonContent className="ion-padding">
				<IonList inset={true}>
					<IonItem>
						<IonInput
							label="Username"
							value={newUsername}
							onIonChange={(e) => setNewUsername(`${e.target.value}`)}
						/>
					</IonItem>
					<IonItem>
						<IonInput
							type="password"
							label="Password"
							value={newPassword}
							onIonChange={(e) => setNewPassword(`${e.target.value}`)}
						>
							<IonInputPasswordToggle slot="end"></IonInputPasswordToggle>
						</IonInput>
					</IonItem>
				</IonList>
				<IonNote color="medium" class="ion-margin-horizontal">
					Until OAuth is implemented, we currently use Access Token for authentication. To get your token,{" "}
					<a href="https://www.discogs.com/settings/developers">visit the Developer page</a> and copy your
					token, or click Generate if you do not have one.
				</IonNote>
			</IonContent>
		</IonModal>
	)
}

export default Settings
