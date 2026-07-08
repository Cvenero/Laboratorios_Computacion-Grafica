import cv2
import numpy as np
from cvzone.HandTrackingModule import HandDetector
from cvzone import overlayPNG
import random
import time

# Add Text to Center


def addTextToCenter(image, text, font=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1, color=(0, 255, 0), thickness=2, lineType=cv2.LINE_AA, custom_x=None, custom_y=None):
    image = image.copy()
    # Get text dimensions
    text_width, text_height = cv2.getTextSize(
        text, font, fontScale, thickness)[0]

    # Calculate center coordinates
    img_center_x = int(FrameWidth / 2)
    img_center_y = int(boardMaxHeight / 2)

    text_x = img_center_x - int(text_width / 2)
    text_y = img_center_y + int(text_height / 2)

    if not (custom_x == None):
        text_x = custom_x

    if not (custom_y == None):
        text_y = custom_y

    image = cv2.putText(image, text, (text_x, text_y),
                        font, fontScale, color, thickness, lineType)

    return image


# Draw the Board
def updateBoard(image):
    image = image.copy()

    # Draw the Board
    mask = np.zeros_like(image)
    mask = cv2.rectangle(
        mask, (0, 0), (FrameWidth, FrameHeight), (0, 0, 0), -1)
    image = cv2.addWeighted(image, 0.3, mask, 0.7, 0)

    # Draw Bottom Rectangle
    mask = cv2.rectangle(mask, (0, boardMaxHeight),
                         (FrameWidth, FrameHeight), (252, 5, 244), -1)
    image = cv2.addWeighted(image, 1, mask, 1, 0)

    # Draw the Gamer Lines
    image = cv2.line(image, (boardWidth, 0),
                     (boardWidth, boardMaxHeight), (252, 5, 244), 1)
    image = cv2.line(image, (FrameWidth-boardWidth, 0),
                     (FrameWidth-boardWidth, boardMaxHeight), (252, 5, 244), 1)

    # Add Points
    image = cv2.putText(image, str(leftPoint), (boardWidth - 25,
                        boardMaxHeight + 90), cv2.FONT_HERSHEY_SIMPLEX, 3, (255, 255, 255), 5)
    image = cv2.putText(image, str(rightPoint), (FrameWidth -
                        boardWidth - 25, boardMaxHeight + 90), cv2.FONT_HERSHEY_SIMPLEX, 3, (255, 255, 255), 5)

    # Add the Ball
    image = overlayPNG(image, ballImg, [ballPosX, ballPosY])

    # Add "How to Start Game" text if Game is not Started
    if not (gameStarted):
        image = addTextToCenter(image, "Show Both hands to Start the Game", color=(
            255, 255, 255), custom_y=boardMaxHeight+70)
    else:
        image = addTextToCenter(image, "Play with Index Fingers", color=(
            255, 255, 255), custom_y=boardMaxHeight+70)

    return image

# Carga las imágenes fuera del bucle
bat_left = cv2.imread("asset/bat_left.png", cv2.IMREAD_UNCHANGED)
bat_right = cv2.imread("asset/bat_right.png", cv2.IMREAD_UNCHANGED)

def maskOnlyHands(img, hands, background):
    mask = np.zeros(img.shape[:2], dtype=np.uint8)

    for hand in hands:
        lmList = hand["lmList"]
        points = np.array([[p[0], p[1]] for p in lmList], dtype=np.int32)
        hull = cv2.convexHull(points)
        cv2.fillConvexPoly(mask, hull, 255)

    mask3 = cv2.cvtColor(mask, cv2.COLOR_GRAY2BGR)
    result = np.where(mask3 == 255, img, background)
    return result


# Update the Bat
def updateBat(img, idxPos, type_hand):
    # Rieles fijos:
    # Si es mano izquierda, X = 50. Si es derecha, X = 910
    x_pos = 50 if type_hand == "Left" else 910
    
    # Restringimos el movimiento vertical (y) para que no se salga de la pantalla
    # 120 es el alto de tu bate, 540 el alto de la pantalla
    y_pos = idxPos[1] - 60 # 60 es la mitad de 120 (para centrar el bate en el dedo)
    y_pos = max(0, min(y_pos, 540 - 120)) 
    
    # Superponemos la imagen
    bat_img = bat_left if type_hand == "Left" else bat_right
    img = overlayPNG(img, bat_img, [x_pos, y_pos])
    
    return img, y_pos # Retornamos la posición Y para calcular la colisión


# Global & Const Variables
boardMaxHeight = 420  # y -> 0 - 420
boardWidth = 80  # x -> 80 - WIDTH - 80

FrameWidth = 960
FrameHeight = 540

ballWH = 50
ballPosX, ballPosY = (FrameWidth // 2) - (ballWH //
                                          2), (boardMaxHeight // 2) - (ballWH // 2)
ballSpeed = 16
# For start, Randomly choose the ball direction
ballSpeedX = random.choice([ballSpeed, -ballSpeed])
ballSpeedY = random.choice([ballSpeed, -ballSpeed])
speedUpEvery = 8  # seconds

batHeight = 100
batWidth = 30

# Inicializamos las Y de los bates (centrados en la pantalla)
leftBatY = 250 
rightBatY = 250
rightBatPosY = [0, 0]

leftPoint = 0
rightPoint = 0

gameStarted = False
gameOver = False

startTime = time.time()

# Ball
ballImg = cv2.imread("./asset/ball.png", cv2.IMREAD_UNCHANGED)

# Initializing Hand Tracker
detector = HandDetector(detectionCon=0.5)

# Video Capture
cap_vid = cv2.VideoCapture(1)
cap_vid.set(cv2.CAP_PROP_FRAME_WIDTH, FrameWidth)
cap_vid.set(cv2.CAP_PROP_FRAME_HEIGHT, FrameHeight)
windowName = "Ping Pong"
cv2.namedWindow(windowName, cv2.WINDOW_NORMAL) # Permite redimensionar manualmente
cv2.resizeWindow(windowName, FrameWidth, FrameHeight) # Fuerza el tamaño exacto

while cap_vid.isOpened():
    ret, frame = cap_vid.read()
    
    if not ret:
        continue

    frame = cv2.resize(frame, (FrameWidth, FrameHeight)) 
    
    img = cv2.flip(frame, 1)

    # draw=True if Hand Needs to be drawn
    handsT, _ = detector.findHands(img, flipType=False, draw=True)
    hands = []
    for hand in handsT:
        print(hand["type"], hand["center"])
        lmList = hand["lmList"]
        cx = hand["center"][0]  # posición horizontal del centro de la mano
        hand["type"] = "Left" if cx < FrameWidth // 2 else "Right"

    hands = handsT  # ya no hace falta deduplicar por tipo, ahora se basa en posición real

    background = cv2.imread("asset/fondo.png")
    background = cv2.resize(background, (FrameWidth, FrameHeight))

    img = maskOnlyHands(img, hands, background)

    # Draw The Boardpython
    img = updateBoard(img)

    # 2. Control de inicio
    if (len(hands) == 2):
        gameStarted = True

    # Draw The Board
    img = updateBoard(img)

    # Depuración: verificar cuántas manos ve justo antes del bucle
    print(f"DEBUG: Cantidad de manos detectadas en este frame: {len(handsT)}")
    
    # ... (después de limpiar la lista 'hands')
    
    # Procesar manos limpias
    for hand in hands:
        fingers = detector.fingersUp(hand)
        lmList = hand["lmList"]
        idxPos = lmList[8]
        img, batY = updateBat(img, idxPos, hand["type"])

        if fingers[1] == 1:            
            
            # Guardar posición para colisión
            if hand["type"] == "Left":
                leftBatY = batY
            else:
                rightBatY = batY

    """
    1. In Below condition, we are first checking if the Ball is in any territory (Left / Right) by Checking it touches or crosses the border --- and then checking if the ball's center is between the Y positions of any Bat. Then change the direction
    2. Then checking if the ball is out of the field, if it is, then game over and point increased
    """
    # Left Bat hit
    if (ballPosX < boardWidth) and (leftBatY < ballPosY < leftBatY + 120):
        ballSpeedX = abs(ballSpeedX) # Asegura que la pelota salga a la derecha
        ballPosX += 30

    # Right Bat hit
    elif (ballPosX + ballWH > FrameWidth - boardWidth) and (rightBatY < ballPosY < rightBatY + 120):
        ballSpeedX = -abs(ballSpeedX) # Asegura que la pelota salga a la izquierda
        ballPosX -= 30

    # Ball out on the Left side (Right player scores)
    if ballPosX < 0:
        rightPoint += 1
        gameOver = True

    # Ball out on the Right side (Left player scores)
    elif ballPosX + ballWH > FrameWidth:
        leftPoint += 1
        gameOver = True


    # If Game is Over
    if (gameOver):
        gameStarted = False
        gameOver = False
        ballSpeedX = random.choice([ballSpeed, -ballSpeed])
        ballSpeedY = random.choice([ballSpeed, -ballSpeed])
        ballPosX, ballPosY = (FrameWidth // 2) - (ballWH //
                                                  2), (boardMaxHeight // 2) - (ballWH // 2)

        imgGO = addTextToCenter(
            img, "Game Over", fontScale=2, color=(255, 255, 255), thickness=3)

        cv2.imshow(windowName, imgGO)
        cv2.waitKey(2000)
   
    # Check if Ball Hits the Wall!
    if (ballPosY <= 0) or (ballPosY + ballWH >= boardMaxHeight):
        ballSpeedY = -ballSpeedY

    if (gameStarted):
        ballPosX += ballSpeedX
        ballPosY += ballSpeedY

    # Speed up after every 15 seconds
    if (time.time() - startTime >= 15):
        if (ballSpeedX < 0):
            ballSpeedX -= 1
        else:
            ballSpeedX += 1

        if (ballSpeedY < 0):
            ballSpeedY -= 1
        else:
            ballSpeedY += 1

        startTime = time.time()

    cv2.imshow(windowName, img)
    if (cv2.waitKey(1) & 0xFF == 27):
        break
    if (cv2.getWindowProperty(windowName, cv2.WND_PROP_VISIBLE) < 1):
        break


# Release Camera
cap_vid.release()
cv2.destroyAllWindows()