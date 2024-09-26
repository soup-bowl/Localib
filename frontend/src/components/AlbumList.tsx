import { IonAvatar, IonChip, IonIcon, IonItem, IonLabel, IonList, IonListHeader, IonText } from "@ionic/react"
import { disc } from "ionicons/icons"
import { IReleases } from "../api"
import "./AlbumList.css"

interface Props {
	data: IReleases[]
	title?: string
	onClickAlbum: (album: IReleases) => void
}

const AlbumList: React.FC<Props> = ({ data, title = undefined, onClickAlbum }) => (
	<IonList lines="full">
		{title && (
			<IonListHeader>
				<IonLabel>{title}</IonLabel>
			</IonListHeader>
		)}
		{data.map((album, index) => {
			return (
				<IonItem key={index} className="album-list-item" onClick={() => onClickAlbum(album)}>
					<IonAvatar aria-hidden="true" slot="start">
						<img alt="" src={album.image_base64 ? album.image_base64 : album.basic_information.thumb} />
					</IonAvatar>
					<IonLabel>
						<strong>{album.basic_information.title}</strong>
						<br />
						<IonText>{album.basic_information.artists.map((artist) => artist.name).join(", ")}</IonText>
					</IonLabel>
					<IonChip slot="end">
						<IonIcon icon={disc} />
						<IonLabel>{album.basic_information.formats[0].name}</IonLabel>
					</IonChip>
				</IonItem>
			)
		})}
	</IonList>
)

export default AlbumList
