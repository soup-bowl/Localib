import { IonAvatar, IonItem, IonLabel, IonList, IonText } from "@ionic/react"
import { IReleases } from "../api"

interface Props {
	data: IReleases[]
	onClickAlbum: (album: IReleases) => void
}

const AlbumList: React.FC<Props> = ({ data, onClickAlbum }) => {
	return (
		<IonList lines="full">
			{data.map((album, index) => (
				<IonItem key={index} onClick={() => onClickAlbum(album)}>
					<IonAvatar aria-hidden="true" slot="start">
						<img alt="" src={album.basic_information.thumb} />
					</IonAvatar>
					<IonLabel>
						<strong>{album.basic_information.title}</strong>
						<br />
						<IonText>{album.basic_information.artists[0].name}</IonText>
					</IonLabel>
				</IonItem>
			))}
		</IonList>
	)
}

export default AlbumList
